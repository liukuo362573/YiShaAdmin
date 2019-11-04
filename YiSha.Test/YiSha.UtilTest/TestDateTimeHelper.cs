using System;
using NUnit.Framework;
using YiSha.Util;

namespace Tests
{
    public class TestDateTimeHelper
    {
        [Test]
        public void TestFormatTime()
        {
            long ticks = 333333000;     // Environment.TickCount;

            string result = DateTimeHelper.FormatTime(ticks);

            Assert.AreEqual("03 天 20 小时 35 分 33 秒", result);
        }
    }
}