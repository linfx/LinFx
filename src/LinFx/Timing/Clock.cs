using System;

namespace LinFx.Timing
{
    public static class Clock
    {
        /// <summary>
        /// Gets Now using current <see cref="Provider"/>.
        /// </summary>
        public static DateTime Now => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local);

        /// <summary>
        /// Gets Now using current <see cref="Provider"/>.
        /// </summary>
        public static DateTimeOffset Now2 => DateTime.Now;
    }
}
