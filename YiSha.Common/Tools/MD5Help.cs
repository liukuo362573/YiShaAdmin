using System.Security.Cryptography;
using System.Text;

namespace YiSha.Common
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class MD5Help
    {
        /// <summary>
        /// 获得MD5加密
        /// </summary>
        /// <param name="content">要加密的文本</param>
        /// <param name="isUpper">是否大写</param>
        /// <returns>返回加密获得文件</returns>
        public static string GetMd5(string content, bool isUpper = false)
        {
            using var md5 = MD5.Create();
            //16
            var toByte = Encoding.UTF8.GetBytes(content);
            var toByteHash = md5.ComputeHash(toByte);
            var strResult = BitConverter.ToString(toByteHash);
            strResult = strResult.Replace("-", "");
            //大小写转换
            var toMd5 = isUpper ? strResult.ToUpper() : strResult.ToLower();
            //返回
            return toMd5;
        }
    }
}