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
        public static double GetUnixTimestamp(DateTime date)
        {
            //Int64 result = 0;
            //var start = new DateTime(1970, 1, 1, 0, 0, 0, date.Kind);
            //result = Convert.ToInt64((date - start).TotalSeconds);
            //return result;

            //double intResult = 0;
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //intResult = (date - startTime).TotalSeconds;
            //return intResult;

            return date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
