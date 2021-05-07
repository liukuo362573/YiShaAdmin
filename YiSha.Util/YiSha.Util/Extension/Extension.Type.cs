using System;

namespace YiSha.Util.Extension
{
    public static class TypeExtensions
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