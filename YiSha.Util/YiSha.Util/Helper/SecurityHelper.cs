using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace YiSha.Util.Helper
{
    public static class SecurityHelper
    {
        /// <summary>
        /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="bit">位数，一般取值16 或 32</param>
        /// <returns>返回的加密后的字符串</returns>
        [SuppressMessage("ReSharper", "CA5351")]
        public static string Md5Encrypt(string str, int bit = 32)
        {
            // 参考：https://docs.microsoft.com/zh-cn/dotnet/fundamentals/code-analysis/quality-rules/ca5351
            using var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                sb.Append(i.ToString("x2"));
            }
            return bit == 16 ? sb.ToString().Substring(8, 16).ToLower() : sb.ToString().ToLower();
        }

        /// <summary>
        /// 用SHA256加密字符串，可选择生成16位或者32位的加密字符串
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="bit">位数，一般取值16 或 32</param>
        /// <returns>返回的加密后的字符串</returns>
        public static string Sha256Encrypt(string str, int bit = 32)
        {
            using var hasher = SHA256.Create();
            var hashedDataBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (var i in hashedDataBytes)
            {
                sb.Append(i.ToString("x2"));
            }
            return bit == 16 ? sb.ToString().Substring(8, 16).ToLower() : sb.ToString().ToLower();
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();
        }

        public static bool IsSafeSqlParam(string value)
        {
            return !Regex.IsMatch(value, @"/('(''|[^'])*')|(;)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)/i");
        }
    }
}