using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using YiSha.Util.Extension;
using YiSha.Util.Helper;

namespace YiSha.Data.Extension
{
    public static class DatabasesExtension
    {
        public static IQueryable<T> AppendSort<T>(this IQueryable<T> query, string sort, bool isAsc)
        {
            string[] sortArr = sort.Split(',');
            var type = typeof(T);
            for (int index = 0; index < sortArr.Length; index++)
            {
                string[] oneSortArr = sortArr[index].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (oneSortArr.Length == 2)
                {
                    isAsc = string.Equals(oneSortArr[1], "asc", StringComparison.OrdinalIgnoreCase);
                }
                var property = ReflectionHelper.GetProperties(type).FirstOrDefault(p => string.Equals(p.Name, oneSortArr[0], StringComparison.CurrentCultureIgnoreCase));
                var parameter = Expression.Parameter(type, "t");
                var methodName = isAsc ? index == 0 ? "OrderBy" : "ThenBy" : "OrderByDescending";
                var propertyAccess = Expression.MakeMemberAccess(parameter, property!);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var typeArguments = new[] { type, property.PropertyType };
                var resultExpression = Expression.Call(typeof(Queryable), methodName, typeArguments, query.Expression, Expression.Quote(orderByExpression));

                query = query.Provider.CreateQuery<T>(resultExpression);
            }
            return query;
        }

        public static string GetSql<T>(this IQueryable<T> query)
        {
            using var enumerator = query.Provider.Execute<IEnumerable<T>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");
            return factory.Create().GetCommand(selectExpression).CommandText;
        }

        public static string GetSql(this DbCommand dbCommand)
        {
            var sql = dbCommand.CommandText;
            foreach (DbParameter parameter in dbCommand.Parameters)
            {
                try
                {
                    string value = parameter.DbType switch
                    {
                        DbType.Date => parameter.Value.ParseToString().ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        _ => parameter.Value.ParseToString()
                    };
                    sql = sql.Replace(parameter.ParameterName, value);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            return sql;
        }

        private static object Private(this object obj, string privateField)
        {
            return obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        }

        private static T Private<T>(this object obj, string privateField)
        {
            return (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        }
    }
}