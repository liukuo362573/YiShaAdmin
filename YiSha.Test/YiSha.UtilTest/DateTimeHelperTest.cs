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

            Assert.Equals("03 �� 20 Сʱ 35 �� 33 ��", result);
        }

        [Test]
        public void TestGetUnixTimeStamp()
        {
            DateTime dt = "2019-09-01 17:37:40".ParseToDateTime();
            long result = DateTimeHelper.GetUnixTimeStamp(dt);

            Assert.Equals(1567330660000, result);
        }
    }
}