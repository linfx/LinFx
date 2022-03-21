using System;
using System.Text;

namespace LinFx.Utils;

public static class Base64Utils
{
    /// <summary>
    /// base64 编码
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToBase64String(this string s)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    }

    /// <summary>
    /// base64 编码
    /// </summary>
    /// <param name="inArray"></param>
    /// <returns></returns>
    public static string ToBase64String(this byte[] inArray)
    {
        return Convert.ToBase64String(inArray);
    }

    /// <summary>
    /// base64 解码
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static byte[] ToBase64Bytes(this string s)
    {
        return Convert.FromBase64String(s);
    }
}
