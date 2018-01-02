using Microsoft.Extensions.Logging;
using NLog.Config;
using System;

namespace LinFx.Logging
{
    public class NLogLogger : ILogger
    {
        readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static NLog.LogLevel ToTargetLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Info:
                    return NLog.LogLevel.Info;
                case LogLevel.Warn:
                    return NLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                case LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
            }
            return NLog.LogLevel.Debug;
        }

        void ILogger.Log(LogLevel logLevel, Func<string> messageFunc, Exception exception)
        {
            var targetLogLevel = ToTargetLogLevel(logLevel);
            if (messageFunc == null)
            {
                //logger.IsEnabled(targetLogLevel);
                return;
            }
            logger.Log(targetLogLevel, exception, messageFunc());
        }

        void ILogger.Log(LogLevel logLevel, Exception exception, Func<string> messageFunc)
        {
            var targetLogLevel = ToTargetLogLevel(logLevel);
            if (messageFunc == null)
            {
                //logger.IsEnabled(targetLogLevel);
                return;
            }
            logger.Log(targetLogLevel, exception, messageFunc());
        }
    }

    public static class ConfigureExtensions
    {
        public static ILoggerFactory AddNLog(this ILoggerFactory factory)
        {
            return NLog.Extensions.Logging.ConfigureExtensions.AddNLog(factory);
        }

        public static LoggingConfiguration ConfigureNLog(this ILoggerFactory loggerFactory, string configFileRelativePath)
        {
            return NLog.Extensions.Logging.ConfigureExtensions.ConfigureNLog(loggerFactory, configFileRelativePath);
        }
    }
}