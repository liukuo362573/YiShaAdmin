using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace YiSha.Util.Helper
{
    public static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<string, object> _propCache = new();
        private static readonly ConcurrentDictionary<string, object> _fieldCache = new();

        /// <summary>
        /// 获取实体类键值（缓存）
        /// </summary>
        public static Hashtable GetPropertyInfo<T>(T entity)
        {
            var hashtable = new Hashtable();
            var notMapped = typeof(NotMappedAttribute);
            var props = GetProperties(typeof(T)).Where(p => p.GetCustomAttribute(notMapped, true) is not NotMappedAttribute);
            foreach (var prop in props)
            {
                string name = prop.Name;
                hashtable[name] = prop.GetValue(entity, null);
            }
            return hashtable;
        }

        /// <summary>
        /// 得到类里面的属性集合
        /// </summary>
        public static PropertyInfo[] GetProperties(Type type, string[] columns = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            PropertyInfo[] properties;
            if (_propCache.ContainsKey(type.FullName!))
            {
                properties = _propCache[type.FullName] as PropertyInfo[];
            }
            else
            {
                properties = type.GetProperties();
                _propCache.TryAdd(type.FullName, properties);
            }

            if (columns == null || columns.Length == 0)
            {
                return properties;
            }

            //  按columns顺序返回属性
            return columns.Select(column => properties?.FirstOrDefault(p => p.Name == column))
                          .Where(columnProperty => columnProperty != null).ToArray();
        }

        /// <summary>
        /// 得到类里面的字段集合
        /// </summary>
        public static FieldInfo[] GetFields(Type type, string[] columns = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            FieldInfo[] fields;
            if (_fieldCache.ContainsKey(type.FullName!))
            {
                fields = _fieldCache[type.FullName] as FieldInfo[];
            }
            else
            {
                fields = type.GetFields();
                _fieldCache.TryAdd(type.FullName, fields);
            }

            if (columns == null || columns.Length == 0)
            {
                return fields;
            }

            //  按columns顺序返回属性
            return columns.Select(column => fields?.FirstOrDefault(f => f.Name == column))
                          .Where(columnProperty => columnProperty != null).ToArray();
        }

        /// <summary>
        /// 得到对应类型的值
        /// </summary>
        public static object GetValue(object value, Type propertyType)
        {
            var trueType = propertyType.GetUnderlyingType();
            if (trueType.IsEnum)
            {
                return System.Enum.ToObject(trueType, value);
            }
            if (trueType == typeof(DateTime))
            {
                var _ = DateTime.TryParse(value?.ToString(), out var result);
                return result;
            }
            return Convert.ChangeType(value, trueType);
        }
    }

    public static class TypeExtension
    {
        /// <summary>
        /// 返回指定nullable类型的底层类型参数
        /// </summary>
        public static Type GetUnderlyingType(this Type type)
        {
            while (true)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // nullable type, check if the nested type is simple.
                    type = type.GetGenericArguments()[0];
                    continue;
                }

                return type;
            }
        }

        public static bool IsElementaryType(this Type type)
        {
            var underlyingType = type.GetUnderlyingType();
            return underlyingType.IsPrimitive || underlyingType.IsEnum || underlyingType == typeof(string) || underlyingType == typeof(decimal);
        }
    }
}