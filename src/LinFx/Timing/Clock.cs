using System;

namespace LinFx.Timing
{
    public static class Clock
    {
        /// <summary>
        /// Gets Now using current <see cref="Provider"/>.
        /// </summary>
        public static DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets Now using current <see cref="Provider"/>.
        /// </summary>
        public static DateTimeOffset Now2 => DateTime.Now;
    }
}
