using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// ormats and writes an exception log message.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        public static void LogException(this ILogger logger, Exception ex, LogLevel? level = null)
        {
            //var selectedLevel = level ?? ex.GetLogLevel();

            //logger.LogWithLevel(selectedLevel, ex.Message, ex);
            //LogKnownProperties(logger, ex, selectedLevel);
            //LogSelfLogging(logger, ex);
            //LogData(logger, ex, selectedLevel);
        }
    }
}
