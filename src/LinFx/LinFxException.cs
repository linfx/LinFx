using System;

namespace LinFx
{
    public class LinFxException : Exception
    {
        public LinFxException()
        {
        }

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
        public ResultException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}
