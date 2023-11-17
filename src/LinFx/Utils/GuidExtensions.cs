namespace LinFx.Utils;

public static class GuidExtensions
{
    /// <summary>
    /// 根据GUID获取16位的唯一字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string To16String(this Guid id)
    {
        long i = 1;
        foreach (byte b in id.ToByteArray())
            i *= b + 1;
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }
}
