using System;
using NUnit.Framework;
using YiSha.Util;

namespace YiSha.UtilTest
{
    public class ComputerHelperTest
    {
        [Test]
        public void TestGetComputerInfo()
        {
            ComputerInfo computerInfo = ComputerHelper.GetComputerInfo();

            Assert.That(string.IsNullOrWhiteSpace(computerInfo.CPURate));
            Assert.That(string.IsNullOrWhiteSpace(computerInfo.RAMRate));
            Assert.That(string.IsNullOrWhiteSpace(computerInfo.TotalRAM));
            Assert.That(string.IsNullOrWhiteSpace(computerInfo.RunTime));
        }
    }
}
