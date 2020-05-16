using System;
using NUnit.Framework;
using YiSha.Util;

namespace YiSha.UtilTest
{
    public class SecurityHelperTest
    {
        [Test]
        public void TestMD5Encrypt()
        {
            string password = "123456";
            string result = SecurityHelper.MD5Encrypt(password);

            Assert.AreEqual("e10adc3949ba59abbe56e057f20f883e", result);
        }
    }
}
