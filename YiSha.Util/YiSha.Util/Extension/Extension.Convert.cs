using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using YiSha.Util.Helper;

namespace YiSha.Util.Extension
{
    public static partial class Extensions
    {
        /// <summary>
        /// 将object转换为int，若转换失败，则返回0。不抛出异常。
        /// </summary>
        public static int ParseToInt(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将object转换为long，若转换失败，则返回0。不抛出异常。
        /// </summary>
        public static long ParseToLong(this object o)
        {
            return long.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为short，若转换失败，则返回0。不抛出异常。
        /// </summary>
        public static short ParseToShort(this object o)
        {
            return short.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为float，若转换失败，则返回0。不抛出异常。
        /// </summary>
        public static float ParseToFloat(this object o)
        {
            return float.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为double，若转换失败，则返回0。不抛出异常。
        /// </summary>
        public static double ParseToDouble(this object o)
        {
            return double.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为demical，若转换失败，则返回0。不抛出异常。
        /// </summary>
        public static decimal ParseToDecimal(this object o)
        {
            return decimal.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为bool，若转换失败，则返回false。不抛出异常。
        /// </summary>
        public static bool ParseToBool(this object o)
        {
            return bool.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为byte，若转换失败，则返回默认值。不抛出异常。
        /// </summary>
        public static byte ParseToByte(this object o)
        {
            return byte.TryParse(o?.ToString(), out var result) ? result : default;
        }

        /// <summary>
        /// 将object转换为string，若转换失败，则返回""。不抛出异常。
        /// </summary>
        public static string ParseToString(this object o)
        {
            try
            {
                return o?.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ParseToStrings<T>(this object o)
        {
            try
            {
                if (o is IEnumerable<T> list)
                {
                    return string.Join(",", list);
                }
                return o?.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 将string转换为DateTime，若转换失败，则返回日期最小值。不抛出异常。
        /// </summary>
        public static DateTime ParseToDateTime(this string s)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    return DateTime.MinValue;
                }
                if (s.Contains("-") || s.Contains("/"))
                {
                    return DateTime.Parse(s);
                }
                return s.Length switch
                {
                    4 => DateTime.ParseExact(s, "yyyy", CultureInfo.CurrentCulture),
                    6 => DateTime.ParseExact(s, "yyyyMM", CultureInfo.CurrentCulture),
                    8 => DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.CurrentCulture),
                    10 => DateTime.ParseExact(s, "yyyyMMddHH", CultureInfo.CurrentCulture),
                    12 => DateTime.ParseExact(s, "yyyyMMddHHmm", CultureInfo.CurrentCulture),
                    14 => DateTime.ParseExact(s, "yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                    _ => DateTime.ParseExact(s, "yyyyMMddHHmmss", CultureInfo.CurrentCulture)
                };
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 强制转换类型
        /// </summary>
        public static IEnumerable<TResult> CastSuper<TResult>(this IEnumerable source)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    yield return (TResult)Convert.ChangeType(item, typeof(TResult));
                }
            }
        }

        public static DataTable ToDataTable<T>(this List<T> list)
        {
            var dt = new DataTable();
            var props = ReflectionHelper.GetProperties(typeof(T));
            foreach (var t in props)
            {
                dt.Columns.Add(t.Name);
            }
            if (list.TryAny())
            {
                dt.BeginLoadData();
                foreach (var values in list.Select(entity => props.Select(p => p.GetValue(entity))))
                {
                    dt.LoadDataRow(values.ToArray(), true);
                }
                dt.EndLoadData();
            }
            return dt;
        }
    }
}