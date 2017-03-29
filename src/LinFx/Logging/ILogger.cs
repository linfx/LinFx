using System;

namespace LinFx.Logging
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null);
    }
}