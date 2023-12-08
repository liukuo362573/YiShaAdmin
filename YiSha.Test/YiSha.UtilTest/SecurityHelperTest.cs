using System;
using NUnit.Framework;
using YiSha.Util;

namespace YiSha.UtilTest
{
    public class SecurityHelperTest
    {
        private string input = "我是谁 ABCD 1234 *=/.";

        [Test]
        public void TestMD5()
        {
            string result = SecurityHelper.MD5ToHex(input);

            Assert.Equals("a7783d564da97a3846f5bf0f6b923d7f", result.ToLower());
        }

        [Test]
        public void TestDES()
        {
            string ciperText = SecurityHelper.DESEncryptToBase64(input);
            string result = SecurityHelper.DESDecryptFromBase64(ciperText);
            Assert.Equals(input, result);

            ciperText = SecurityHelper.DESEncryptToHex(input);
            result = SecurityHelper.DESDecryptFromHex(ciperText);
            Assert.Equals(input, result);
        }

        [Test]
        public void TestAES()
        {
            string ciperText = SecurityHelper.AESEncryptToBase64(input);
            string result = SecurityHelper.AESDecryptFromBase64(ciperText);
            Assert.Equals(input, result);

            ciperText = SecurityHelper.AESEncryptToHex(input);
            result = SecurityHelper.AESDecryptFromHex(ciperText);
            Assert.Equals(input, result);
        }
    }
}
