using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YiSha.Util;

namespace YiSha.DataBase.Extension
{
    /// <summary>
    /// 数据库拓展
    /// </summary>
    public static class DbExtension
    {
        /// <summary>
        /// 将 DataReader 转为 Dynamic
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
        /// 将 IDataReader 转为 List
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
        /// 将 IDataReader 转换为 List
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
                    field.Add(reader.GetName(i));
                }
                List<T> list = new List<T>();
                while (reader.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in DbExtensionReflection.GetProperties(model.GetType()))
                    {
                        if (field.Contains(property.Name))
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
        ///  将 IDataReader 转换为 DataTable
        /// </summary>
        /// <param name="reader">读者</param>
        /// <returns></returns>
        public static DataTable IDataReaderToDataTable(IDataReader reader)
        {
            using (reader)
            {
                DataTable objDataTable = new DataTable("Table");
                int intFieldCount = reader.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    objDataTable.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
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
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static Hashtable GetPropertyInfo<T>(T entity)
        {
            Hashtable ht = new Hashtable();
            PropertyInfo[] props = DbExtensionReflection.GetProperties(entity.GetType());
            foreach (PropertyInfo prop in props)
            {
                bool flag = true;
                foreach (Attribute attr in prop.GetCustomAttributes(true))
                {
                    if (attr is NotMappedAttribute notMapped)
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

        /// <summary>
        /// AppendSort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempData"></param>
        /// <param name="sort"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 这个类对可空类型进行判断转换，要不然会报错
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="conversionType">转换类型</param>
        /// <returns></returns>
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

        /// <summary>
        /// 是否为空数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString()));
        }

        /// <summary>
        /// 获取使用Linq生成的SQL
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
        /// 获取运行时的SQL
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
                        case System.Data.DbType.Date:
                            value = Convert.ToDateTime(parameter.Value).ToString("yyyy-MM-dd HH:mm:ss");
                            break;
                        case System.Data.DbType.AnsiString:
                            break;
                        case System.Data.DbType.Binary:
                            break;
                        case System.Data.DbType.Byte:
                            break;
                        case System.Data.DbType.Boolean:
                            break;
                        case System.Data.DbType.Currency:
                            break;
                        case System.Data.DbType.DateTime:
                            break;
                        case System.Data.DbType.Decimal:
                            break;
                        case System.Data.DbType.Double:
                            break;
                        case System.Data.DbType.Guid:
                            break;
                        case System.Data.DbType.Int16:
                            break;
                        case System.Data.DbType.Int32:
                            break;
                        case System.Data.DbType.Int64:
                            break;
                        case System.Data.DbType.Object:
                            break;
                        case System.Data.DbType.SByte:
                            break;
                        case System.Data.DbType.Single:
                            break;
                        case System.Data.DbType.String:
                            break;
                        case System.Data.DbType.Time:
                            break;
                        case System.Data.DbType.UInt16:
                            break;
                        case System.Data.DbType.UInt32:
                            break;
                        case System.Data.DbType.UInt64:
                            break;
                        case System.Data.DbType.VarNumeric:
                            break;
                        case System.Data.DbType.AnsiStringFixedLength:
                            break;
                        case System.Data.DbType.StringFixedLength:
                            break;
                        case System.Data.DbType.Xml:
                            break;
                        case System.Data.DbType.DateTime2:
                            break;
                        case System.Data.DbType.DateTimeOffset:
                            break;
                        default:
                            value = Convert.ToString(parameter.Value);
                            break;
                    }
                    sql = sql.Replace(parameter.ParameterName, value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return sql;
        }

        /// <summary>
        /// 获取对象的值（私有）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="privateField"></param>
        /// <returns></returns>
        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);

        /// <summary>
        /// 获取对象的值（私有）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="privateField"></param>
        /// <returns></returns>
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }

    /// <summary>
    /// 反射工具（私有）
    /// </summary>
    partial class DbExtensionReflection
    {
        /// <summary>
        /// 同步字典
        /// </summary>
        private static ConcurrentDictionary<string, object> dictCache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 得到类里面的属性集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="columns">列</param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(Type type, string[] columns = null)
        {
            PropertyInfo[] properties = null;
            if (dictCache.ContainsKey(type.FullName))
            {
                properties = dictCache[type.FullName] as PropertyInfo[];
            }
            else
            {
                properties = type.GetProperties();
                dictCache.TryAdd(type.FullName, properties);
            }

            if (columns != null && columns.Length > 0)
            {
                //按columns顺序返回属性
                var columnPropertyList = new List<PropertyInfo>();
                foreach (var column in columns)
                {
                    var columnProperty = properties.Where(p => p.Name == column).FirstOrDefault();
                    if (columnProperty != null)
                    {
                        columnPropertyList.Add(columnProperty);
                    }
                }
                return columnPropertyList.ToArray();
            }
            else
            {
                return properties;
            }
        }
    }
}
