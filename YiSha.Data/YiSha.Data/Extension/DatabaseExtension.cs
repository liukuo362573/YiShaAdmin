using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Data
{
    public static class DatabasesExtension
    {
        /// <summary>
        /// 将DataReader数据转为Dynamic对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static dynamic DataFillDynamic(IDataReader reader)
        {
            using (reader)
            {
                dynamic d = new ExpandoObject();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    try
                    {
                        ((IDictionary<string, object>)d).Add(reader.GetName(i), reader.GetValue(i));
                    }
                    catch
                    {
                        ((IDictionary<string, object>)d).Add(reader.GetName(i), null);
                    }
                }
                return d;
            }
        }

        /// <summary>
        /// 获取模型对象集合
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<dynamic> DataFillDynamicList(IDataReader reader)
        {
            using (reader)
            {
                List<dynamic> list = new List<dynamic>();
                if (reader != null && !reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        list.Add(DataFillDynamic(reader));
                    }
                    reader.Close();
                    reader.Dispose();
                }
                return list;
            }
        }

        /// <summary>
        /// 将IDataReader转换为集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> IDataReaderToList<T>(IDataReader reader)
        {
            using (reader)
            {
                List<string> field = new List<string>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    field.Add(reader.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (reader.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in ReflectionHelper.GetProperties(model.GetType()))
                    {
                        if (field.Contains(property.Name.ToLower()))
                        {
                            if (!IsNullOrDBNull(reader[property.Name]))
                            {
                                property.SetValue(model, HackType(reader[property.Name], property.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                reader.Close();
                reader.Dispose();
                return list;
            }
        }

        /// <summary>
        ///  将IDataReader转换为DataTable
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataTable IDataReaderToDataTable(IDataReader reader)
        {
            using (reader)
            {
                DataTable objDataTable = new DataTable("Table");
                int intFieldCount = reader.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    objDataTable.Columns.Add(reader.GetName(intCounter).ToLower(), reader.GetFieldType(intCounter));
                }
                objDataTable.BeginLoadData();
                object[] objValues = new object[intFieldCount];
                while (reader.Read())
                {
                    reader.GetValues(objValues);
                    objDataTable.LoadDataRow(objValues, true);
                }
                reader.Close();
                reader.Dispose();
                objDataTable.EndLoadData();
                return objDataTable;
            }
        }

        /// <summary>
        /// 获取实体类键值（缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Hashtable GetPropertyInfo<T>(T entity)
        {
            Hashtable ht = new Hashtable();
            PropertyInfo[] props = ReflectionHelper.GetProperties(entity.GetType());
            foreach (PropertyInfo prop in props)
            {
                bool flag = true;
                foreach (Attribute attr in prop.GetCustomAttributes(true))
                {
                    NotMappedAttribute notMapped = attr as NotMappedAttribute;
                    if (notMapped != null)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    string name = prop.Name;
                    object value = prop.GetValue(entity, null);
                    ht[name] = value;
                }
            }
            return ht;
        }

        public static IQueryable<T> AppendSort<T>(IQueryable<T> tempData, string sort, bool isAsc)
        {
            string[] sortArr = sort.Split(',');
            MethodCallExpression resultExpression = null;
            for (int index = 0; index < sortArr.Length; index++)
            {
                string[] oneSortArr = sortArr[index].Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string sortField = oneSortArr[0];
                bool sortAsc = isAsc;
                if (oneSortArr.Length == 2)
                {
                    sortAsc = string.Equals(oneSortArr[1], "asc", StringComparison.OrdinalIgnoreCase) ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = ReflectionHelper.GetProperties(typeof(T)).Where(p => p.Name.ToLower() == sortField.ToLower()).FirstOrDefault();
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                if (index == 0)
                {
                    resultExpression = Expression.Call(typeof(Queryable), sortAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExpression));
                }
                else
                {
                    resultExpression = Expression.Call(typeof(Queryable), sortAsc ? "ThenBy" : "ThenByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExpression));
                }
                tempData = tempData.Provider.CreateQuery<T>(resultExpression);
            }
            return tempData;
        }

        //这个类对可空类型进行判断转换，要不然会报错
        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        public static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }

        /// <summary>
        /// 获取使用Linq生成的Sql
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string GetSql<TEntity>(this IQueryable<TEntity> query)
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        /// <summary>
        /// 获取运行时的Sql
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        public static string GetCommandText(this DbCommand dbCommand)
        {
            var sql = dbCommand.CommandText;
            foreach (DbParameter parameter in dbCommand.Parameters)
            {
                try
                {
                    string value = string.Empty;
                    switch (parameter.DbType)
                    {
                        case DbType.Date:
                            value = parameter.Value.ParseToString().ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                            break;
                        default:
                            value = parameter.Value.ParseToString();
                            break;
                    }
                    sql = sql.Replace(parameter.ParameterName, value);
                }
                catch(Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            return sql;
        }

        #region 私有方法
        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        #endregion
    }
}
