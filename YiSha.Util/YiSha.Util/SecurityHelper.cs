using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace YiSha.Util
{
    public class SecurityHelper
    {
        private static readonly string DESKey = "*change*"; // 8位或者16位
        private static readonly string DESIv = "1change1"; // 8位或者16位

        private static readonly string AESKey = "12345dontusethis"; // 16位或者32位
        private static readonly string AESIv = "youshouldchange!"; // 16位或者32位

        public static byte[] MD5(string input)
        {
            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] byteArr = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(input));
            return byteArr;
        }

        public static string MD5ToHex(string input, int bit = 32)
        {
            byte[] byteArr = MD5(input);
            string result = EncodingHelper.ByteArrToHexDefault(byteArr);
            if (bit == 16)
            {
                return result.Substring(8, 16).ToUpper();
            }
            else
            {
                return result.ToUpper();
            }
        }

        public static byte[] AESEncrypt(string input, string key = "", string iv = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = AESKey;
            }
            if (string.IsNullOrEmpty(iv))
            {
                iv = AESIv;
            }
            try
            {
                var encoding = new ASCIIEncoding();
                var keyByte = encoding.GetBytes(key);
                var ivByte = encoding.GetBytes(iv);
                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(keyByte, ivByte))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))

                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(input);
                            }
                            return msEncrypt.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }

        public static string AESEncryptToBase64(string input, string key = "", string iv = "")
        {
            byte[] byteArr = AESEncrypt(input, key, iv);
            if (byteArr != null)
            {
                return Convert.ToBase64String(byteArr);
            }
            return string.Empty;
        }

        public static string AESEncryptToHex(string input, string key = "", string iv = "")
        {
            byte[] byteArr = AESEncrypt(input, key, iv);
            if (byteArr != null)
            {
                return EncodingHelper.ByteArrToHexDefault(byteArr);
            }
            return string.Empty;
        }

        public static string AESDecrypt(byte[] byteArr, string key = "", string iv = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = AESKey;
            }
            if (string.IsNullOrEmpty(iv))
            {
                iv = AESIv;
            }
            try
            {
                var encoding = new ASCIIEncoding();
                var keyByte = encoding.GetBytes(key);
                var ivByte = encoding.GetBytes(iv);
                using (var aesAlg = Aes.Create())
                {
                    using (var decryptor = aesAlg.CreateDecryptor(keyByte, ivByte))
                    {
                        string result;
                        using (var msDecrypt = new MemoryStream(byteArr))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return string.Empty;
        }

        public static string AESDecryptFromBase64(string cipherText, string key = "", string iv = "")
        {
            var byteArr = Convert.FromBase64String(cipherText);
            return AESDecrypt(byteArr, key, iv);
        }

        public static string AESDecryptFromHex(string cipherText, string key = "", string iv = "")
        {
            var byteArr = EncodingHelper.HexToByteArrDefault(cipherText);
            return AESDecrypt(byteArr, key, iv);
        }

        public static byte[] DESEncrypt(string input, string key = "", string iv = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = DESKey;
            }
            if (string.IsNullOrEmpty(iv))
            {
                iv = DESIv;
            }
            try
            {
                var encoding = new ASCIIEncoding();
                using (DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider())
                {
                    byte[] inputArr = Encoding.UTF8.GetBytes(input);
                    desCryptoServiceProvider.Key = encoding.GetBytes(key);
                    desCryptoServiceProvider.IV = encoding.GetBytes(iv);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(inputArr, 0, inputArr.Length);
                            cryptoStream.FlushFinalBlock();
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }

        public static string DESEncryptToBase64(string input, string key = "", string iv = "")
        {
            byte[] byteArr = DESEncrypt(input, key, iv);
            if (byteArr != null)
            {
                return Convert.ToBase64String(byteArr);
            }
            return string.Empty;
        }

        public static string DESEncryptToHex(string input, string key = "", string iv = "")
        {
            byte[] byteArr = DESEncrypt(input, key, iv);
            if (byteArr != null)
            {
                return EncodingHelper.ByteArrToHexDefault(byteArr);
            }
            return string.Empty;
        }

        public static byte[] DESDecrypt(byte[] byteArr, string key = "", string iv = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = DESKey;
            }
            if (string.IsNullOrEmpty(iv))
            {
                iv = DESIv;
            }
            try
            {
                var encoding = new ASCIIEncoding();
                using (DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider())
                {
                    desCryptoServiceProvider.Key = encoding.GetBytes(key);
                    desCryptoServiceProvider.IV = encoding.GetBytes(iv);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(byteArr, 0, byteArr.Length);
                            cryptoStream.FlushFinalBlock();
                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return null;
        }

        public static string DESDecryptFromBase64(string cipherText, string key = "", string iv = "")
        {
            var byteArr = Convert.FromBase64String(cipherText);
            var result = DESDecrypt(byteArr, key, iv);
            if (result != null)
            {
                return Encoding.UTF8.GetString(result);
            }
            return string.Empty;
        }

        public static string DESDecryptFromHex(string cipherText, string key = "", string iv = "")
        {
            var byteArr = EncodingHelper.HexToByteArrDefault(cipherText);
            var result = DESDecrypt(byteArr, key, iv);
            if (result != null)
            {
                return Encoding.UTF8.GetString(result);
            }
            return string.Empty;
        }

        public static string Base64Encrypt(string encrypt)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(encrypt);
                string base64 = Convert.ToBase64String(bytes);
                return base64;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return string.Empty;
        }

        public static string Base64Decrypt(string decrypt)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(decrypt);
                string base64 = Encoding.UTF8.GetString(bytes);
                return base64;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return string.Empty;
        }

        public static byte[] HMAC_SHA256(string encrypt, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = DESKey;
            }
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] encryptByte = encoding.GetBytes(encrypt);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                return hmacsha256.ComputeHash(encryptByte);
            }
        }

        public static string HMAC_SHA256ToHex(string encrypt, string key = "")
        {
            byte[] hash = HMAC_SHA256(encrypt, key);
            return EncodingHelper.ByteArrToHexDefault(hash);
        }

        public static string GetGuid(bool replaceDash = false)
        {
            string guid = Guid.NewGuid().ToString();
            if (replaceDash)
            {
                guid = guid.Replace("-", string.Empty);
            }
            return guid;
        }

        public static bool IsSafeSqlParam(string value)
        {
            return !Regex.IsMatch(value, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
    }
}
