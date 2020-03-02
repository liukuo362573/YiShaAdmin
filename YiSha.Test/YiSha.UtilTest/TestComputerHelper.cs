using System;
using NUnit.Framework;
using YiSha.Util;

namespace YiSha.UtilTest
{
    public class TestComputerHelper
    {
        [Test]
        public void TestGetComputerInfo()
        {
            ComputerInfo computerInfo = ComputerHelper.GetComputerInfo();

            Assert.IsNotEmpty(computerInfo.CPURate);
            Assert.IsNotEmpty(computerInfo.RAMRate);
            Assert.IsNotEmpty(computerInfo.TotalRAM);
            Assert.IsNotEmpty(computerInfo.RunTime);
        }
    }
}
