using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace YiSha.Util
{
    public static class ValidatorHelper
    {
        #region  验证输入字符串为数字(带小数)
        /// <summary>
        /// 验证输入字符串为带小数点正数
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsNumber(this string str)
        {
            return Regex.IsMatch(str, "^([0]|([1-9]+\\d{0,}?))(.[\\d]+)?$");
        }
        /// <summary>
        /// 验证输入字符串为带小数点正负数
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsNumberic(this string str)
        {
            return Regex.IsMatch(str, "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$");
        }
        #endregion

        #region 验证中国电话格式是否有效，格式010-85849685
        /// <summary>
        /// 验证中国电话格式是否有效，格式010-85849685
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsTel(this string str)
        {
            return Regex.IsMatch(str, @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证输入字符串为电话号码
        /// <summary>
        /// 验证输入字符串为电话号码
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsPhone(this string str)
        {
            return Regex.IsMatch(str, @"(^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)");
            //弱一点的验证：  @"\d{3,4}-\d{7,8}"         
        }
        #endregion

        #region 验证是否是有效传真号码
        /// <summary>
        /// 验证是否是有效传真号码
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsFax(this string str)
        {
            return Regex.IsMatch(str, @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
        }
        #endregion

        #region 验证手机号是否合法
        /// <summary>
        /// 验证手机号是否合法 号段为13,14,15,16,17,18,19  0，86开头将自动识别
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsMobile(this string str)
        {
            if (!str.StartsWith("1"))
            {
                str = str.TrimStart(new char[] { '8', '6', }).TrimStart('0');
            }
            return Regex.IsMatch(str, @"^(13|14|15|16|17|18|19)\d{9}$");
        }
        #endregion

        #region 验证身份证是否有效
        /// <summary>
        /// 验证身份证是否有效
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCard(this string str)
        {
            switch (str.Length)
            {
                case 18:
                    {
                        return str.IsIDCard18();
                    }
                case 15:
                    {
                        return str.IsIDCard15();
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证输入字符串为18位的身份证号码
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCard18(this string str)
        {
            long n = 0;
            if (long.TryParse(str.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(str.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(str.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = str.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = str.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            return arrVarifyCode[y] == str.Substring(17, 1).ToLower();
        }
        /// <summary>
        /// 验证输入字符串为15位的身份证号码
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCard15(this string str)
        {
            long n = 0;
            if (long.TryParse(str, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(str.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = str.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            return DateTime.TryParse(birth, out time) != false;
        }
        #endregion

        #region 验证是否是有效邮箱地址
        /// <summary>
        /// 验证是否是有效邮箱地址
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 验证是否只含有汉字
        /// <summary>
        /// 验证是否只含有汉字
        /// </summary>
        /// <param name="strln">输入字符</param>
        /// <returns></returns>
        public static bool IsOnlyChinese(this string strln)
        {
            return Regex.IsMatch(strln, @"^[\u4e00-\u9fa5]+$");
        }
        #endregion

        #region 是否有多余的字符 防止SQL注入
        /// <summary>
        /// 是否有多余的字符 防止SQL注入
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsBadString(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            //列举一些特殊字符串
            const string badChars = "@,*,#,$,!,+,',=,--,%,^,&,?,(,), <,>,[,],{,},/,\\,;,:,\",\"\",delete,update,drop,alert,select";
            var arraryBadChar = badChars.Split(',');
            return arraryBadChar.Any(t => !str.Contains(t));
        }
        #endregion

        #region 是否由数字、26个英文字母或者下划线組成的字串
        /// <summary>
        /// 是否由数字、26个英文字母或者下划线組成的字串 
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsNzx(this string str)
        {
            return Regex.Match(str, "^[0-9a-zA-Z_]+$").Success;
        }
        #endregion

        #region 由数字、26个英文字母、汉字組成的字串
        /// <summary>
        /// 由数字、26个英文字母、汉字組成的字串
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsSzzmChinese(this string str)
        {
            return Regex.Match(str, @"^[0-9a-zA-Z\u4e00-\u9fa5]+$").Success;
        }
        #endregion

        #region 由数字、26个英文字母組成的字串
        /// <summary>
        /// 是否由数字、26个英文字母組成的字串
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsSzzm(this string str)
        {
            return Regex.Match(str, @"^[0-9a-zA-Z]+$").Success;
        }
        #endregion

        #region 验证输入字符串为邮政编码
        /// <summary>
        /// 验证输入字符串为邮政编码
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsPostCode(this string str)
        {
            return Regex.IsMatch(str, @"\d{6}");
        }
        #endregion

        #region 检查对象的输入长度
        /// <summary>
        /// 检查对象的输入长度
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <param name="length">指定的长度</param>
        /// <returns>false 太长，true -太短</returns>
        public static bool CheckLength(this string str, int length)
        {
            if (str.Length > length)
            {
                return false;//长度太长
            }
            return str.Length >= length;
        }

        #endregion

        #region 判断用户输入是否为日期
        /// <summary>
        /// 判断用户输入是否为日期
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        /// <remarks>
        /// 可判断格式如下（其中-可替换为/，不影响验证)
        /// YYYY | YYYY-MM | YYYY-MM-DD | YYYY-MM-DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF
        /// </remarks>
        public static bool IsDateTime(this string str)
        {
            if (null == str)
            {
                return false;
            }
            const string regexDate = @"[1-2]{1}[0-9]{3}((-|\/|\.){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/|\.){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
            if (Regex.IsMatch(str, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性
                int indexY = -1;
                int indexM = -1;
                int indexD = -1;
                if (-1 != (indexY = str.IndexOf("-", StringComparison.Ordinal)))
                {
                    indexM = str.IndexOf("-", indexY + 1, StringComparison.Ordinal);
                    indexD = str.IndexOf(":", StringComparison.Ordinal);
                }
                else
                {
                    indexY = str.IndexOf("/", StringComparison.Ordinal);
                    indexM = str.IndexOf("/", indexY + 1, StringComparison.Ordinal);
                    indexD = str.IndexOf(":", StringComparison.Ordinal);
                }
                //不包含日期部分，直接返回true
                if (-1 == indexM)
                    return true;
                if (-1 == indexD)
                {
                    indexD = str.Length + 3;
                }
                int iYear = Convert.ToInt32(str.Substring(0, indexY));
                int iMonth = Convert.ToInt32(str.Substring(indexY + 1, indexM - indexY - 1));
                int iDate = Convert.ToInt32(str.Substring(indexM + 1, indexD - indexM - 4));
                //判断月份日期
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                        return true;
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                            return true;
                    }
                    else
                    {
                        //闰年
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                                return true;
                        }
                        else
                        {
                            if (iDate < 29)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
