using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Data.Extension
{
    public static class DataReaderExtension
    {
        public static DataTable ToDataTable(this IDataReader reader)
        {
            using (reader)
            {
                var dt = reader.GetSchemaTable();
                if (dt != null)
                {
                    dt.BeginLoadData();
                    var values = new object[reader.FieldCount];
                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        dt.LoadDataRow(values, true);
                    }
                    dt.EndLoadData();
                }
                return dt;
            }
        }

        public static List<T> ToList<T>(this IDataReader reader, string sql = null)
        {
            using (reader)
            {
                var list = new List<T>();

                if (typeof(T).IsElementaryType())
                {
                    while (reader.Read())
                    {
                        var item = (T)GetValue<T>(reader);
                        list.Add(item);
                    }
                    return list;
                }

                if (typeof(T) == typeof(object))
                {
                    while (reader.Read())
                    {
                        var item = GetDynamic(reader);
                        list.Add(item);
                    }
                    return list;
                }

                var mapping = ReaderMapperCache<T>.GetMapper(reader, sql);
                while (reader.Read())
                {
                    var item = mapping(reader as DbDataReader);
                    list.Add(item);
                }
                return list;
            }
        }

        public static IEnumerable<T> AsEnumerable<T>(this IDataReader reader, string sql = null)
        {
            using (reader)
            {
                if (typeof(T).IsElementaryType())
                {
                    while (reader.Read())
                    {
                        yield return (T)GetValue<T>(reader);
                    }
                }

                if (typeof(T) == typeof(object))
                {
                    while (reader.Read())
                    {
                        yield return GetDynamic(reader);
                    }
                }

                var mapping = ReaderMapperCache<T>.GetMapper(reader, sql);
                while (reader.Read())
                {
                    yield return mapping(reader as DbDataReader);
                }
            }
        }

        public static T ToInstance<T>(this IDataReader reader, string sql = null)
        {
            using (reader)
            {
                if (typeof(T).IsElementaryType())
                {
                    if (reader.Read())
                    {
                        return (T)GetValue<T>(reader);
                    }
                }
                else if (typeof(T) == typeof(object))
                {
                    if (reader.Read())
                    {
                        return GetDynamic(reader);
                    }
                }
                else
                {
                    var mapping = ReaderMapperCache<T>.GetMapper(reader, sql);
                    if (reader.Read())
                    {
                        return mapping(reader);
                    }
                }
            }
            return default;
        }

        private static dynamic GetDynamic(IDataRecord reader)
        {
            dynamic result = new ExpandoObject();
            var dict = result as IDictionary<string, object>;
            for (var i = 0; i < reader.FieldCount; i++)
            {
                try
                {
                    dict.Add(reader.GetName(i), reader[i]);
                }
                catch
                {
                    dict.Add(reader.GetName(i), null);
                }
            }
            return dict;
        }

        private static object GetValue<T>(IDataRecord reader)
        {
            return reader[0] is DBNull ? default(T) : ReflectionHelper.GetValue(reader[0], typeof(T));
        }
    }

    internal class MappingMemberInfo
    {
        public int Index { get; set; }

        public string ColumnName { get; set; }

        public PropertyInfo PropertyInfo { get; set; }
    }

    internal class CacheIdentity
    {
        private readonly Type _type;
        private readonly string _sql;
        private readonly int _hashCode;
        private readonly IEnumerable<MappingMemberInfo> _memberInfos;

        /*
         * 使用SQL语句和映射类型作为缓存
         * 可以考虑更多的属性作为缓存标识
         * 例如添加连接字符串就可以实现多数据库缓存
         */
        public CacheIdentity(string sql, Type type, IEnumerable<MappingMemberInfo> memberInfos)
        {
            _sql = sql;
            _type = type;
            _memberInfos = memberInfos;
            unchecked
            {
                _hashCode = 17;
                _hashCode = _hashCode * 23 + (sql?.GetHashCode() ?? 0);
                _hashCode = _hashCode * 23 + (type?.GetHashCode() ?? 0);
                _hashCode = _hashCode * 23 + (memberInfos?.GetHashCode() ?? 0);
            }
        }

        public override int GetHashCode() => _hashCode;

        public override bool Equals(object obj)
        {
            var other = obj as CacheIdentity;
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            return _type == other._type && _memberInfos == other._memberInfos && _sql == other._sql;
        }
    }

    internal sealed class ReaderMapperCache<T> : Dictionary<int, Func<IDataRecord, T>>
    {
        private static readonly ConcurrentDictionary<CacheIdentity, Func<IDataRecord, T>> _funcCaches = new ConcurrentDictionary<CacheIdentity, Func<IDataRecord, T>>();

        internal static Func<IDataRecord, T> GetMapper(IDataReader reader, string sql)
        {
            var memberInfos = GetMemberInfos(reader);
            var identity = new CacheIdentity(sql, typeof(T), memberInfos);
            if (!_funcCaches.ContainsKey(identity))
            {
                _funcCaches.TryAdd(identity, SimpleReaderMapper(memberInfos));
            }
            var _ = _funcCaches.TryGetValue(identity, out var value);
            return value;
        }

        private static Func<IDataRecord, T> SimpleReaderMapper(IEnumerable<MappingMemberInfo> memberInfos)
        {
            var type = typeof(T);
            var expressions = new List<Expression>();
            var getItem = typeof(IDataRecord).GetMethod("get_Item", new[] { typeof(int) });

            // Method(DbDataReader reader)
            var readerExp = Expression.Parameter(typeof(IDataRecord), "reader");

            // var obj = new T();
            var objExp = Expression.Variable(type, "obj");
            expressions.Add(Expression.Assign(objExp, Expression.New(type)));

            /*
             * if (reader[index] is not DBNull)
             *     obj.property = (T)reader[index];
             */
            expressions.AddRange(from member in memberInfos
                                 let propType = member.PropertyInfo.PropertyType
                                 let getItemExp = Expression.Call(readerExp, getItem, Expression.Constant(member.Index))
                                 let propExp = Expression.Property(objExp, member.PropertyInfo.Name)
                                 let notExp = Expression.Not(Expression.TypeIs(getItemExp, typeof(DBNull)))
                                 let convertExp = ConvertExpression(getItemExp, propType)
                                 select Expression.IfThen(notExp, Expression.Assign(propExp, convertExp)));
            expressions.Add(objExp); // return obj;
            var body = Expression.Block(new[] { objExp }, expressions);
            return Expression.Lambda<Func<IDataRecord, T>>(body, readerExp).Compile();
        }

        private static IEnumerable<MappingMemberInfo> GetMemberInfos(IDataRecord reader)
        {
            var props = ReflectionHelper.GetProperties(typeof(T));
            var members = Enumerable.Range(0, reader.FieldCount)
                                    .Select(reader.GetName)
                                    .Select((name, i) => new MappingMemberInfo
                                    {
                                        Index = i,
                                        ColumnName = name,
                                        PropertyInfo = props.First(p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase))
                                    });
            return members;
        }

        private static UnaryExpression ConvertExpression(Expression getItemExp, Type type)
        {
            var underlyingType = type.GetUnderlyingType();
            if (underlyingType.IsEnum)
            {
                var method = typeof(System.Enum).GetMethod("ToObject", new[] { typeof(Type), typeof(object) });
                var callExp = Expression.Call(method, new[] { Expression.Constant(underlyingType), getItemExp });
                return Expression.Unbox(callExp, type); // object 是引用类型，Enum 是值类型，转换时需要拆箱
            }
            /*
             * 主要用于解决 long 和 int 这种转换的问题
             * 比如 select count(1)，返回值是 long
             * 但是我们通常会用 int 来装 count 结果
             * 所以这里需要使用强制转换
             */
            var changeType = typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(Type) });
            var changeTypeExp = Expression.Call(changeType, new[] { getItemExp, Expression.Constant(underlyingType) });
            return Expression.Convert(changeTypeExp, type);
        }
    }
}