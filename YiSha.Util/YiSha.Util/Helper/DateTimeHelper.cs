using System;

namespace YiSha.Util.Helper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 毫秒转天时分秒
        /// </summary>
        public static string FormatTime(long ms)
        {
            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;
            int dd = hh * 24;

            long day = ms / dd;
            long hour = (ms - day * dd) / hh;
            long minute = (ms - day * dd - hour * hh) / mi;
            long second = (ms - day * dd - hour * hh - minute * mi) / ss;
            // long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

            string sDay = day < 10 ? "0" + day : "" + day; //天
            string sHour = hour < 10 ? "0" + hour : "" + hour; //小时
            string sMinute = minute < 10 ? "0" + minute : "" + minute; //分钟
            string sSecond = second < 10 ? "0" + second : "" + second; //秒
            // string sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond; //毫秒
            // sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;

            return $"{sDay} 天 {sHour} 小时 {sMinute} 分 {sSecond} 秒";
        }

        /// <summary>
        /// 获取unix时间戳
        /// </summary>
        public static long GetUnixTimeStamp(DateTime dt)
        {
            return ((DateTimeOffset)dt).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 获取日期天的最小时间
        /// </summary>
        public static DateTime GetDayMinDate(DateTime dt)
        {
            return new(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        /// <summary>
        /// 获取日期天的最大时间
        /// </summary>
        public static DateTime GetDayMaxDate(DateTime dt)
        {
            return new(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        /// <summary>
        /// 获取日期天的最大时间
        /// </summary>
        public static string FormatDateTime(DateTime? dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }
            if (dt.Value.Year == DateTime.Now.Year)
            {
                return dt.Value.ToString("MM-dd HH:mm");
            }
            return dt.Value.ToString("yyyy-MM-dd HH:mm");
        }
    }
}