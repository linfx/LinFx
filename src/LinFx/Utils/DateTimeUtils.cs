namespace LinFx.Utils;

public static class DateTimeUtils
{
    /// <summary>
    /// 得到当前的unix时间戳
    /// </summary>
    /// <param name="date">当前时间日期</param>
    /// <returns></returns>
    public static double ToUnixTimestamp(DateTime date) => date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

    public static long ToUnixTimeSeconds(this DateTime dt) => new DateTimeOffset(dt).ToUnixTimeSeconds();
}

/// <summary>
/// Extension methods for <see cref="DayOfWeekExtensions"/>.
/// </summary>
public static class DayOfWeekExtensions
{
    /// <summary>
    /// Check if given <see cref="DayOfWeek"/> value is weekend.
    /// </summary>
    public static bool IsWeekend(this DayOfWeek dayOfWeek) => dayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday);

    /// <summary>
    /// Check if given <see cref="DayOfWeek"/> value is weekday.
    /// </summary>
    public static bool IsWeekday(this DayOfWeek dayOfWeek) => dayOfWeek.IsIn(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday);
}
