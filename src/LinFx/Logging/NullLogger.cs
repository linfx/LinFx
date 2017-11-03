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
