using System.Text;

namespace System;

public static class Base64Utils
{
    /// <summary>
    /// base64 编码
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToBase64String(this string s) => Convert.ToBase64String(Encoding.UTF8.GetBytes(s));

    /// <summary>
    /// base64 编码
    /// </summary>
    /// <param name="inArray"></param>
    /// <returns></returns>
    public static string ToBase64String(this byte[] inArray) => Convert.ToBase64String(inArray);

    /// <summary>
    /// base64 解码
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static byte[] ToBase64Bytes(this string s) => Convert.FromBase64String(s);
}
