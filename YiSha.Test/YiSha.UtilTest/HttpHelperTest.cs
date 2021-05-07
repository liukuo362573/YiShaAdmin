using NUnit.Framework;
using System.Text;
using YiSha.Util.Helper;

namespace YiSha.UtilTest
{
    public class HttpHelperTest
    {
        [Test]
        public void TestMD5Encrypt()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // 注册Encoding

            HttpResult httpResult = new HttpHelper().GetHtml(new HttpItem
            {
                Url = "http://whois.pconline.com.cn/ip.jsp?ip=117.64.156.76",
                ContentType = "text/html; charset=gb2312"
            });
        }
    }
}