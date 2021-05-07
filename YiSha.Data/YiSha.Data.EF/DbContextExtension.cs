using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using YiSha.Data.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Data.EF
{
    public static class DbContextExtension
    {
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        public static string DeleteSql(string tableName)
        {
            return $"DELETE FROM {tableName}";
        }

        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        public static (string sql, DbParameter parameter) DeleteSql(string tableName, string propertyName, object propertyValue)
        {
            var parameter = DbParameterHelper.CreateDbParameter($"@{propertyName}", propertyValue);
            var sql = $"DELETE FROM {tableName} WHERE {propertyName} = {parameter.ParameterName}";
            return (sql, parameter);
        }

        /// <summary>
        /// 拼接批量删除SQL语句
        /// </summary>
        public static (string sql, DbParameter[] parameters) DeleteSql(string tableName, string propertyName, object[] propertyValue)
        {
            var parameters = DbParameterHelper.CreateDbParameters($"@{propertyName}", propertyValue);
            var sql = $"DELETE FROM {tableName} WHERE {propertyName} IN ({string.Join(',', parameters.Select(x => x.ParameterName))})";
            return (sql, parameters);
        }

        /// <summary>
        /// 获取实体映射对象
        /// </summary>
        public static IEntityType GetEntityType<T>(Microsoft.EntityFrameworkCore.DbContext dbContext) where T : class
        {
            return dbContext.Model.FindEntityType(typeof(T));
        }

        /// <summary>
        /// 存储过程语句
        /// </summary>
        public static string BuilderProc(string procName, params DbParameter[] dbParameter)
        {
            return $"EXEC {procName} {string.Join(',', dbParameter.Select(x => $"{x.ParameterName}"))}";
        }

        /// <summary>
        /// 把null设置成对应属性类型的默认值
        /// </summary>
        public static void SetEntityDefaultValue(Microsoft.EntityFrameworkCore.DbContext dbContext)
        {
            foreach (var entry in dbContext.ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                var type = entry.Entity.GetType();
                var props = ReflectionHelper.GetProperties(type).Where(p => p.Name != "Id");
                foreach (var prop in props)
                {
                    object value = prop.GetValue(entry.Entity, null);
                    if (value == null)
                    {
                        string typeName = GetPropertyTypeName(prop);
                        var defaultValue = GetPropertyDefaultValue(typeName);
                        prop.SetValue(entry.Entity, defaultValue);
                    }
                    else if (value.ToString() == DateTime.MinValue.ToString(CultureInfo.InvariantCulture))
                    {
                        // sql server datetime类型的的范围不到0001-01-01，所以转成1970-01-01
                        prop.SetValue(entry.Entity, GlobalConstant.DefaultTime);
                    }
                }
            }
        }

        private static string GetPropertyTypeName(PropertyInfo prop)
        {
            return prop.PropertyType.GenericTypeArguments.Length > 0 ? prop.PropertyType.GenericTypeArguments[0].Name : prop.PropertyType.Name;
        }

        private static object GetPropertyDefaultValue(string typeName)
        {
            return typeName switch
            {
                "Boolean" => default(bool),
                "Char" => default(char),
                "SByte" => default(sbyte),
                "Byte" => default(char),
                "Int16" => default(short),
                "UInt16" => default(ushort),
                "Int32" => default(int),
                "UInt32" => default(uint),
                "Int64" => default(long),
                "UInt64" => default(ulong),
                "Single" => default(float),
                "Double" => default(double),
                "Decimal" => default(decimal),
                "DateTime" => GlobalConstant.DefaultTime,
                "String" => string.Empty,
                _ => default
            };
        }
    }
}