using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Util
{
    public class EncodingHelper
    {
        private static string HexStr = "0123456789abcdef";
        private static char[] HexCharArr = HexStr.ToCharArray();

        public static string ByteArrToHex(byte[] btArr)
        {
            char[] strArr = new char[btArr.Length * 2];
            int i = 0;
            foreach (byte bt in btArr)
            {
                strArr[i++] = HexCharArr[bt >> 4 & 0xf];
                strArr[i++] = HexCharArr[bt & 0xf];
            }
            return new string(strArr);
        }

        public static byte[] HexToByteArr(string hexStr)
        {
            char[] charArr = hexStr.ToCharArray();
            byte[] btArr = new byte[charArr.Length / 2];
            int index = 0;
            for (int i = 0; i < charArr.Length; i++)
            {
                int highBit = HexStr.IndexOf(charArr[i]);
                int lowBit = HexStr.IndexOf(charArr[++i]);
                btArr[index] = (byte)(highBit << 4 | lowBit);
                index++;
            }
            return btArr;
        }

        public static string ByteArrToHexDefault(byte[] btArr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in btArr)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static byte[] HexToByteArrDefault(string hexStr)
        {
            byte[] inputArr = new byte[hexStr.Length / 2];
            for (int i = 0; i < hexStr.Length / 2; i++)
            {
                int v = Convert.ToInt32(hexStr.Substring(i * 2, 2), 16);
                inputArr[i] = (byte)v;
            }
            return inputArr;
        }
    }
}
