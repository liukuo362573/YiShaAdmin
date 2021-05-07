using NUnit.Framework;
using YiSha.Util.Helper;

namespace YiSha.UtilTest
{
    public class ComputerHelperTest
    {
        [Test]
        public void TestGetComputerInfo()
        {
            ComputerInfo computerInfo = ComputerHelper.GetComputerInfo();

            Assert.IsNotEmpty(computerInfo.CpuRate);
            Assert.IsNotEmpty(computerInfo.RamRate);
            Assert.IsNotEmpty(computerInfo.TotalRam);
            Assert.IsNotEmpty(computerInfo.RunTime);
        }
    }
}