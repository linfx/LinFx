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

    public class ResultException : Exception
    {
        public ResultException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
