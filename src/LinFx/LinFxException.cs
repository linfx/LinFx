using System;

namespace LinFx
{
    public class LinFxException : Exception
    {
        public LinFxException() { }

        public LinFxException(string message)
            : base(message)
        {
        }

        public LinFxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
