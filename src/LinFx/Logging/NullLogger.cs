using System;

namespace LinFx.Logging
{
    public class NullLogger : ILogger
    {
        public static NullLogger Instance => new NullLogger();

        public void Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
        }

		public void Log(LogLevel logLevel, Exception exception, Func<string> messageFunc = null)
		{
			throw new NotImplementedException();
		}
	}
}

//namespace LinFx.Logging
//{
//    public class NLogLogger : ILogger
//    {
//        readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

//        private static NLog.LogLevel ToTargetLogLevel(LogLevel logLevel)
//        {
//            switch (logLevel)
//            {
//                case LogLevel.Trace:
//                    return NLog.LogLevel.Trace;
//                case LogLevel.Debug:
//                    return NLog.LogLevel.Debug;
//                case LogLevel.Info:
//                    return NLog.LogLevel.Info;
//                case LogLevel.Warn:
//                    return NLog.LogLevel.Warn;
//                case LogLevel.Error:
//                    return NLog.LogLevel.Error;
//                case LogLevel.Fatal:
//                    return NLog.LogLevel.Fatal;
//            }
//            return NLog.LogLevel.Debug;
//        }

//        void ILogger.Log(LogLevel logLevel, Func<string> messageFunc, Exception exception)
//        {
//            var targetLogLevel = ToTargetLogLevel(logLevel);
//            if (messageFunc == null)
//            {
//                //logger.IsEnabled(targetLogLevel);
//                return;
//            }
//            logger.Log(targetLogLevel, exception, messageFunc());
//        }

//		void ILogger.Log(LogLevel logLevel, Exception exception, Func<string> messageFunc)
//		{
//			var targetLogLevel = ToTargetLogLevel(logLevel);
//			if(messageFunc == null)
//			{
//				//logger.IsEnabled(targetLogLevel);
//				return;
//			}
//			logger.Log(targetLogLevel, exception, messageFunc());
//		}
//	}
//}
