using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util.Extension;

namespace YiSha.Util
{
    public class CommonHelper
    {
        /// <summary>
        /// sys_role转成SysRole
        /// </summary>
        /// <param name="lowercase"></param>
        /// <returns></returns>
        public static string ConvertToUppercase(string lowercase)
        {
            lowercase = lowercase.ParseToString();
            StringBuilder sb = new StringBuilder();
            string[] arr = lowercase.Split('_');
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i][0].ToString().ToUpper() + arr[i].Substring(1));
            }
            return sb.ToString();
        }

        /// <summary>
        /// SysUserService转成sysUserService
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstLetterLowercase(string instanceName)
        {
            instanceName = instanceName.ParseToString();
            if (!instanceName.IsEmpty())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(instanceName[0].ToString().ToLower() + instanceName.Substring(1));
                return sb.ToString();
            }
            else
            {
                return instanceName;
            }
        }

        /// <summary>
        /// sys_menu_authorize变成MenuAuthorize
        /// </summary>
        public static string GetClassNamePrefix(string tableName)
        {
            string[] arr = tableName.Split('_');
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < arr.Length; i++)
            {
                sb.Append(arr[i][0].ToString().ToUpper() + arr[i].Substring(1));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetCustomValueWhenEmpty(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubString(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (value.Length > length)
            {
                value = value.Substring(0, length);
            }
            return value;
        }

        /// <summary>
        /// 字符串转指定类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(string value, char split)
        {
            T[] arr = value.Split(new string[] { split.ToString() }, StringSplitOptions.RemoveEmptyEntries).CastSuper<T>().ToArray();
            return arr;
        }

        public static string IsActiveClass(string url, string current)
        {
            if (url == "/" && current.ToLower() == "/home/index")
            {
                return "active";
            }
            else if (url.ToLower() == current.ToLower())
            {
                return "active";
            }
            return string.Empty;
        }
    }
}
