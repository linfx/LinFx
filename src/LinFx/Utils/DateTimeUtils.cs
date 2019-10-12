using System;

namespace LinFx.Utils
{
    public static class DateTimeUtils
    {
        public static object TimeZone { get; private set; }

        /// <summary>
        /// 得到当前的unix时间戳
        /// </summary>
        /// <param name="date">当前时间日期</param>
        /// <returns></returns>
        public static double ToUnixTimestamp(DateTime date)
        {
            return date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static long ToUnixTimeSeconds(this DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeSeconds();
        }
    }
}
