using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using YiSha.Util.Helper;

namespace YiSha.Util.Extension
{
    public static partial class Extensions
    {
        /// <summary>
        /// 枚举成员转成dictionary类型
        /// </summary>
        public static Dictionary<int, string> EnumToDictionary(this Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            var dictionary = new Dictionary<int, string>();
            var fields = ReflectionHelper.GetFields(enumType);
            foreach (var info in fields)
            {
                if (info.FieldType.IsEnum)
                {
                    var enumValue = enumType.InvokeMember(info.Name, BindingFlags.GetField, null, null, null).ParseToInt();
                    var enumAttributes = info.GetCustomAttributes<DescriptionAttribute>().ToArray();
                    var enumDescription = enumAttributes.Any() ? enumAttributes[0].Description : info.Name;
                    dictionary.Add(enumValue, enumDescription);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 枚举成员转成键值对Json字符串
        /// </summary>
        public static string EnumToDictionaryString(this Type enumType)
        {
            var dictionaryList = EnumToDictionary(enumType).ToList();
            return JsonConvert.SerializeObject(dictionaryList);
        }

        /// <summary>
        /// 获取枚举值对应的描述
        /// </summary>
        public static string GetDescription(this System.Enum enumType)
        {
            if (enumType == null) throw new ArgumentNullException(nameof(enumType));
            return enumType.GetType()
                           .GetField(enumType.ToString())?
                           .GetCustomAttribute<DescriptionAttribute>()?
                           .Description ?? enumType.ToString();
        }

        /// <summary>
        /// 根据值获取枚举的描述
        /// </summary>
        public static string GetDescriptionByEnum<T>(this object o)
        {
            var e = System.Enum.Parse(typeof(T), o.ParseToString()) as System.Enum;
            return e.GetDescription();
        }
    }

    public static class EnumHelper
    {
        /// <summary>
        /// 为有Flags特性的枚举类提供拆解后用逗号连接的值字符串
        /// </summary>
        public static string GetValueString<T>(T source) where T : System.Enum
        {
            var builder = new StringBuilder();
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                if ((source.ParseToInt() & value.ParseToInt()) != 0)
                {
                    builder.Append(value.ToString("d") + ",");
                }
            }
            return builder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 为有Flags特性的枚举类提供拆解后用逗号连接的Description字符串
        /// </summary>
        public static string GetDescription<T>(T source) where T : System.Enum
        {
            var builder = new StringBuilder();
            foreach (T value in System.Enum.GetValues(typeof(T)))
            {
                if ((source.ParseToInt() & value.ParseToInt()) != 0)
                {
                    builder.Append(value.GetDescription() + ",");
                }
            }
            return builder.ToString().TrimEnd(',');
        }
    }
}