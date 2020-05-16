using System;
using NUnit.Framework;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.UtilTest
{
    public class DateTimeHelperTest
    {
        [Test]
        public void TestFormatTime()
        {
            long ticks = 333333000;     // Environment.TickCount;
            string result = DateTimeHelper.FormatTime(ticks);

            Assert.AreEqual("03 天 20 小时 35 分 33 秒", result);
        }

        [Test]
        public void TestGetUnixTimeStamp()
        {
            DateTime dt = "2019-09-01 17:37:40".ParseToDateTime();
            long result = DateTimeHelper.GetUnixTimeStamp(dt);

            Assert.AreEqual(1567330660000, result);
        }
    }
}