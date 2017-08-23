using System;

namespace LinFx.Logging
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null);

		void Log(LogLevel logLevel, Exception exception, Func<string> messageFunc = null);
	}

    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}