using Microsoft.AspNetCore.JsonPatch.Operations;
using NLog.LayoutRenderers;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;

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

        public static Expression<Func<T, bool>> True<T>()
        { return param => true; }

        public static Expression<Func<T, bool>> False<T>()
        { return param => false; }

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
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
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
            if (input == null) return x => true;
            List<QueryCompareAttribute> cndQueryList = new List<QueryCompareAttribute>();
            var allProperty = typeof(TT).GetProperties();
            foreach (var p in allProperty)
            {
                var compareAttr = p.GetCustomAttribute<QueryCompareAttribute>();
                if (compareAttr != null && compareAttr.IsIgnore) continue;
                var val = p.GetValue(input,null);
                if ( val == null || string.IsNullOrEmpty(val.ToString())) continue;

                //查找和实体映射的属性名和值以及操作符
                string fieldName = p.Name;
                CompareEnum compareType = CompareEnum.Equals;
                if (compareAttr != null)
                {
                    fieldName = string.IsNullOrWhiteSpace(compareAttr.FieldName) ? p.Name : compareAttr.FieldName;
                    compareType = compareAttr.Compare;
                }
                cndQueryList.Add(new QueryCompareAttribute()
                {
                    FieldName = fieldName,
                    Value = val,
                    Compare = compareType,
                });
            }
            var exp = BuildAndAlsoLambda<T>(cndQueryList);
            return exp;
        }

        /// <summary>
        /// ParameterRebinder
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The ParameterExpression map
            /// </summary>
            private readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private static bool IsNullable(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Expression GetExpression(ParameterExpression parameter, QueryCompareAttribute condition)
        {
            var propertyParam = Expression.Property(parameter, condition.FieldName);

            var propertyInfo = propertyParam.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new MissingMemberException(nameof(QueryCompareAttribute), condition.FieldName);

            //Support Nullable<>
            var realPropertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                propertyParam = Expression.Property(propertyParam, "Value");

            //Support IEnumerable && IEnumerable<T>
            if (condition.Compare != CompareEnum.In && condition.Compare != CompareEnum.NotIn)
            {
                condition.Value = Convert.ChangeType(condition.Value, realPropertyType);
            }
            else
            {
                var typeOfValue = condition.Value.GetType();
                var typeOfList = typeof(IEnumerable<>).MakeGenericType(realPropertyType);
                if (typeOfValue.IsGenericType && typeOfList.IsAssignableFrom(typeOfValue))
                    condition.Value = typeof(Enumerable)
                    .GetMethod("ToArray", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .MakeGenericMethod(realPropertyType)
                    .Invoke(null, new object[] { condition.Value });
            }

            var constantParam = Expression.Constant(condition.Value);
            switch (condition.Compare)
            {
                case CompareEnum.Equals:
                    return Expression.Equal(propertyParam, constantParam);

                case CompareEnum.NotEquals:
                    return Expression.NotEqual(propertyParam, constantParam);

                case CompareEnum.Contains:
                    return Expression.Call(propertyParam, "Contains", null, constantParam); ;
                case CompareEnum.NotContains:
                    return Expression.Not(Expression.Call(propertyParam, "Contains", null, constantParam));

                case CompareEnum.StartsWith:
                    return Expression.Call(propertyParam, "StartsWith", null, constantParam);

                case CompareEnum.EndsWith:
                    return Expression.Call(propertyParam, "EndsWith", null, constantParam);

                case CompareEnum.GreaterThan:
                    return Expression.GreaterThan(propertyParam, constantParam);

                case CompareEnum.GreaterThanOrEquals:
                    return Expression.GreaterThanOrEqual(propertyParam, constantParam);

                case CompareEnum.LessThan:
                    return Expression.LessThan(propertyParam, constantParam);

                case CompareEnum.LessThanOrEquals:
                    return Expression.LessThanOrEqual(propertyParam, constantParam);

                case CompareEnum.In:
                    return Expression.Call(typeof(Enumerable), "Contains", new Type[] { realPropertyType }, new Expression[] { constantParam, propertyParam });

                case CompareEnum.NotIn:
                    return Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new Type[] { realPropertyType }, new Expression[] { constantParam, propertyParam }));

                default:
                    return Expression.Equal(propertyParam, constantParam);
            }
        }

        public static Expression<Func<T, bool>> BuildAndAlsoLambda<T>(IEnumerable<QueryCompareAttribute> conditions)
        {
            if (conditions == null || !conditions.Any())
                return x => true;

            var parameter = Expression.Parameter(typeof(T), "x");
            var simpleExps = conditions
                .ToList()
                .Select(c => GetExpression(parameter, c))
                .ToList();

            var exp = simpleExps.Aggregate<Expression, Expression>(null, (left, right) =>
                left == null ? right : Expression.AndAlso(left, right));
            return Expression.Lambda<Func<T, bool>>(exp, parameter);
        }

        public static Expression<Func<T, bool>> BuildOrElseLambda<T>(IEnumerable<QueryCompareAttribute> conditions)
        {
            if (conditions == null || !conditions.Any())
                return x => true;

            var parameter = Expression.Parameter(typeof(T), "x");
            var simpleExps = conditions
                .ToList()
                .Select(c => GetExpression(parameter, c))
                .ToList();

            var exp = simpleExps.Aggregate<Expression, Expression>(null, (left, right) =>
                left == null ? right : Expression.OrElse(left, right));
            return Expression.Lambda<Func<T, bool>>(exp, parameter);
        }
    }

    /// <summary>
    /// 查询比较符枚举
    /// </summary>
    public enum CompareEnum
    {
        Equals = 1,
        NotEquals = 2,
        LessThan = 3,
        LessThanOrEquals = 4,
        GreaterThan = 5,
        GreaterThanOrEquals = 6,
        In = 7,
        NotIn = 8,
        Contains = 9,
        NotContains = 10,
        StartsWith = 11,
        EndsWith = 12
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryCompareAttribute : Attribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 表达式拼接运算符号
        /// </summary>
        public CompareEnum Compare = CompareEnum.Equals;

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 是否忽略该属性
        /// </summary>
        public bool IsIgnore { get; set; }
    }
}