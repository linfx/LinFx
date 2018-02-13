using System;
using System.Text;

namespace LinFx.Utils
{
    public static class Base64Utils
    {
        public static string ToBase64String(this string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }

        public static byte[] ToBase64Bytes(this string s)
        {
            return Convert.FromBase64String(s);
        }

        public static string ToBase64String(this byte[] inArray)
        {
            return Convert.ToBase64String(inArray);
        }
    }
}
