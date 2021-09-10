using System;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception ex, LogLevel? level = default)
        {
            var selectedLevel = level ?? ex.GetLogLevel();
            logger.Log(selectedLevel, ex, ex.Message);
            LogKnownProperties(logger, ex, selectedLevel);
            LogSelfLogging(logger, ex);
            LogData(logger, ex, selectedLevel);
        }

        private static void LogKnownProperties(ILogger logger, Exception exception, LogLevel logLevel)
        {
            //if (exception is IHasErrorCode exceptionWithErrorCode)
            //{
            //    logger.LogWithLevel(logLevel, "Code:" + exceptionWithErrorCode.Code);
            //}

            //if (exception is IHasErrorDetails exceptionWithErrorDetails)
            //{
            //    logger.LogWithLevel(logLevel, "Details:" + exceptionWithErrorDetails.Details);
            //}
        }

        private static void LogData(ILogger logger, Exception exception, LogLevel logLevel)
        {
            if (exception.Data == null || exception.Data.Count <= 0)
            {
                return;
            }

            var exceptionData = new StringBuilder();
            exceptionData.AppendLine("---------- Exception Data ----------");
            foreach (var key in exception.Data.Keys)
            {
                exceptionData.AppendLine($"{key} = {exception.Data[key]}");
            }

            logger.Log(logLevel, exceptionData.ToString());
        }

        private static void LogSelfLogging(ILogger logger, Exception exception)
        {
            //var loggingExceptions = new List<IExceptionWithSelfLogging>();

            //if (exception is IExceptionWithSelfLogging)
            //{
            //    loggingExceptions.Add(exception as IExceptionWithSelfLogging);
            //}
            //else if (exception is AggregateException && exception.InnerException != null)
            //{
            //    var aggException = exception as AggregateException;
            //    if (aggException.InnerException is IExceptionWithSelfLogging)
            //    {
            //        loggingExceptions.Add(aggException.InnerException as IExceptionWithSelfLogging);
            //    }

            //    foreach (var innerException in aggException.InnerExceptions)
            //    {
            //        if (innerException is IExceptionWithSelfLogging)
            //        {
            //            loggingExceptions.AddIfNotContains(innerException as IExceptionWithSelfLogging);
            //        }
            //    }
            //}

            //foreach (var ex in loggingExceptions)
            //{
            //    ex.Log(logger);
            //}
        }
    }
}
