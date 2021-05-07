using System;
using System.IO;
using System.Linq;
using YiSha.Util.Extension;

namespace YiSha.Util.Helper
{
    public static class TextHelper
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        public static string GetCustomValue(string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        public static string GetSubString(string value, int length, bool ellipsis = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                if (ellipsis)
                {
                    value += "...";
                }
            }
            return value;
        }

        /// <summary>
        /// 字符串转指定类型数组
        /// </summary>
        public static T[] SplitToArray<T>(string value, char split)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.Split(split, StringSplitOptions.RemoveEmptyEntries).CastSuper<T>().ToArray();
        }

        /// <summary>
        /// BASE64转WAV
        /// </summary>
        public static void Base64ToWav(string base64, string fileName)
        {
            if (base64?.Length > 0)
            {
                byte[] array = Convert.FromBase64String(base64);
                using var mp3File = File.Create(fileName, array.Length);
                mp3File.Write(array, 0, array.Length);
            }
        }
    }
}