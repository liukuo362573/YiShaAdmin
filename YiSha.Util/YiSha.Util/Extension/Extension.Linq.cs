using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Util.Extension
{
    public static class LinqExtensions
    {
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
        public static Expression<T> ToLambda<T>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<T>(body, parameters);
        }

        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>
        /// 组合And
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }
        /// <summary>
        /// 组合Or
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// #jhzou0616 根据数据类型获取实体表达式(目前仅支持string/日期范围 的条件筛选),字段属性名称必须匹配** 日期范围字段必须以 xxxStart/xxxEnd 结尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetExpressionItems<T, TT>(TT input)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            Expression expression = Expression.Constant(true);

            var properties = typeof(TT).GetProperties();

            var targetProperties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                //字符类型筛选
                if (property.PropertyType == typeof(string))
                {


                    var value = property.GetValue(input);

                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        var propertyExpression = Expression.Property(parameter, property.Name);
                        var valueExpression = Expression.Constant(value);
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(propertyExpression, containsMethod, valueExpression);
                        expression = Expression.AndAlso(expression, containsExpression);
                    }
                }
                //日期类型筛选
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    var value = property.GetValue(input);

                    if (value != null)
                    {
                        // 开始日期筛选
                        if (property.Name.Contains("Start"))
                        {
                            var realPropertyName = property.Name.Substring(0, property.Name.Length - "Start".Length);
                            var propertyExpression = Expression.Property(parameter, realPropertyName);
                            var entityfield = targetProperties.FirstOrDefault(s => s.Name == realPropertyName);
                            var startDateTime = (DateTime)value;

                            if (entityfield.PropertyType == typeof(DateTime?))
                            {
                                var greaterThanOrEqualExpression = Expression.GreaterThanOrEqual(propertyExpression, Expression.Constant(startDateTime, typeof(DateTime?)));
                                expression = Expression.AndAlso(expression, greaterThanOrEqualExpression);
                            }
                            else
                            {
                                var greaterThanOrEqualExpression = Expression.GreaterThanOrEqual(propertyExpression, Expression.Constant(startDateTime));
                                expression = Expression.AndAlso(expression, greaterThanOrEqualExpression);
                            }

                        }
                        // 结束日期筛选
                        else if (property.Name.Contains("End"))
                        {
                            var realPropertyName = property.Name.Substring(0, property.Name.Length - "End".Length);
                            var propertyExpression = Expression.Property(parameter, realPropertyName);
                            var entityfield = targetProperties.FirstOrDefault(s => s.Name == realPropertyName);
                            var endDateTime = ((DateTime)value).Date.AddDays(1).AddTicks(-1);

                            if (entityfield.PropertyType == typeof(DateTime?))
                            {
                                var lessThanOrEqualExpression = Expression.LessThanOrEqual(propertyExpression, Expression.Constant(endDateTime, typeof(DateTime?)));
                                expression = Expression.AndAlso(expression, lessThanOrEqualExpression);
                            }
                            else
                            {
                                var lessThanOrEqualExpression = Expression.LessThanOrEqual(propertyExpression, Expression.Constant(endDateTime));
                                expression = Expression.AndAlso(expression, lessThanOrEqualExpression);
                            }

                        }
                        // 其他日期属性
                        else
                        {
                            var propertyExpression = Expression.Property(parameter, property.Name);
                            var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(value));
                            expression = Expression.AndAlso(expression, equalExpression);
                        }
                    }

                }
                //数值类型
                else if (property.PropertyType == typeof(int?) || property.PropertyType == typeof(int))
                {
                    var value = property.GetValue(input);
                    var propertyExpression = Expression.Property(parameter, property.Name);
                    var entityfield = targetProperties.FirstOrDefault(s => s.Name == property.Name);

                    if (value != null)
                    {
                        if (entityfield.PropertyType == typeof(int?))
                        {
                            var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(value, typeof(int?)));
                            expression = Expression.AndAlso(expression, equalExpression);
                        }
                        else
                        {
                            var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(value));
                            expression = Expression.AndAlso(expression, equalExpression);
                        }

                    }

                }
                //float
                else if (property.PropertyType == typeof(float?) || property.PropertyType == typeof(float))
                {
                    var value = property.GetValue(input);
                    var propertyExpression = Expression.Property(parameter, property.Name);
                    var entityfield = targetProperties.FirstOrDefault(s => s.Name == property.Name);

                    if (value != null)
                    {
                        if (entityfield.PropertyType == typeof(float?))
                        {
                            var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(value, typeof(float?)));
                            expression = Expression.AndAlso(expression, equalExpression);
                        }
                        else
                        {
                            var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(value));
                            expression = Expression.AndAlso(expression, equalExpression);
                        }

                    }

                }

            }

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }

        /// <summary>
        /// ParameterRebinder
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The ParameterExpression map
            /// </summary>
            readonly Dictionary<ParameterExpression, ParameterExpression> map;
            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }
            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            /// <summary>
            /// Visits the parameter.
            /// </summary>
            /// <param name="p">The p.</param>
            /// <returns>Expression</returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }

        }
    }
}
