using System;
using System.Text;

namespace LinFx.Utils
{
    public static class HexUtils
    {
        /// <summary>
        /// 16进制原码字符串转字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] HexStringToBytes(this string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        public static string ToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();
            if (bytes != null || bytes.Length > 0)
            {
                foreach (var item in bytes)
                {
                    sb.Append(string.Format("{0:x2}", item));
                }
            }
            return sb.ToString();
        }
    }
}
