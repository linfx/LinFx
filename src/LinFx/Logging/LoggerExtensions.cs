using System;
using System.Globalization;

namespace LinFx.Logging
{
    public static class LoggerExtensions
    {
        #region Debug
        public static void Debug(this ILogger logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Debug, messageFunc);
        }

        public static void Debug(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(LogLevel.Debug, message.AsFunc());
            }
        }

        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogFormat(LogLevel.Debug, message, args);
            }
        }

        public static void Debug(this ILogger logger, string message, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(LogLevel.Debug, message.AsFunc(), exception);
            }
        }
        #endregion

        #region Error

        public static void Error(this ILogger logger, Func<string> messageFunc)
        {
            logger.Log(LogLevel.Error, messageFunc);
        }

        public static void Error(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.Log(LogLevel.Error, message.AsFunc());
            }
        }

        public static void Error(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogFormat(LogLevel.Error, message, args);
            }
        }

        public static void Error(this ILogger logger, string message, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.Log(LogLevel.Error, message.AsFunc(), exception);
            }
        }

        #endregion

        #region Fatal

        public static void Fatal(this ILogger logger, Func<string> messageFunc)
        {
            logger.Log(LogLevel.Fatal, messageFunc);
        }

        public static void Fatal(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Fatal))
            {
                logger.Log(LogLevel.Fatal, message.AsFunc());
            }
        }

        public static void FatalFormat(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Fatal))
            {
                logger.LogFormat(LogLevel.Fatal, message, args);
            }
        }

        public static void FatalException(this ILogger logger, string message, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Fatal))
            {
                logger.Log(LogLevel.Fatal, message.AsFunc(), exception);
            }
        }
        #endregion

        #region Info
        public static void Info(this ILogger logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Info, messageFunc);
        }

        public static void Info(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Info))
            {
                logger.Log(LogLevel.Info, message.AsFunc());
            }
        }

        public static void InfoFormat(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Info))
            {
                logger.LogFormat(LogLevel.Info, message, args);
            }
        }

        public static void InfoException(this ILogger logger, string message, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Info))
            {
                logger.Log(LogLevel.Info, message.AsFunc(), exception);
            }
        }
        #endregion

        #region Trace
        public static void Trace(this ILogger logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Trace, messageFunc);
        }

        public static void Trace(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.Log(LogLevel.Trace, message.AsFunc());
            }
        }

        public static void TraceFormat(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogFormat(LogLevel.Trace, message, args);
            }
        }

        public static void TraceException(this ILogger logger, string message, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.Log(LogLevel.Trace, message.AsFunc(), exception);
            }
        }
        #endregion

        #region Warn
        public static void Warn(this ILogger logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(LogLevel.Warn, messageFunc);
        }

        public static void Warn(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogLevel.Warn))
            {
                logger.Log(LogLevel.Warn, message.AsFunc());
            }
        }

        public static void Warn(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Warn))
            {
                logger.LogFormat(LogLevel.Warn, message, args);
            }
        }

        public static void Warn(this ILogger logger, string message, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Warn))
            {
                logger.Log(LogLevel.Warn, message.AsFunc(), exception);
            }
        }
        #endregion

        #region Public Method
        public static bool IsEnabled(this ILogger logger, LogLevel level)
        {
            return true;
        } 
        #endregion

        #region Private Method
        private static void GuardAgainstNullLogger(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
        }

        private static void LogFormat(this ILogger logger, LogLevel logLevel, string message, params object[] args)
        {
            var result = string.Format(CultureInfo.InvariantCulture, message, args);
            logger.Log(logLevel, result.AsFunc());
        }

        private static Func<T> AsFunc<T>(this T value) where T : class
        {
            return value.Return;
        }

        private static T Return<T>(this T value)
        {
            return value;
        } 
        #endregion
    }
}