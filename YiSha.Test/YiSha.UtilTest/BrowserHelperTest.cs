using System;
using NUnit.Framework;
using YiSha.Util;
using YiSha.Util.Browser;

namespace YiSha.UtilTest
{
    public class BrowserHelperTest
    {
        [Test]
        public void TestGetBrwoserInfo()
        {
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0";
            string browserInfo = BrowserHelper.GetBrwoserInfo(userAgent);
            Assert.AreEqual(browserInfo, "Firefox 65.0");

            userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            browserInfo = BrowserHelper.GetBrwoserInfo(userAgent);
            Assert.AreEqual(browserInfo, "IE 11.0");

            userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36";
            browserInfo = BrowserHelper.GetBrwoserInfo(userAgent);
            Assert.AreEqual(browserInfo, "Chrome 72.0.3626.109");

            userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_1_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.1 Mobile/15E148 Safari/604.1";
            browserInfo = BrowserHelper.GetBrwoserInfo(userAgent);
            Assert.AreEqual(browserInfo, "Safari 604.1");

            userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18362";
            browserInfo = BrowserHelper.GetBrwoserInfo(userAgent);
            Assert.AreEqual(browserInfo, "Edge 18.18362");
        }
    }
}
