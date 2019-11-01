using System;

namespace LinFx
{
    public class UserFriendlyException : LinFxException
    {
        /// <summary>
        /// An arbitrary error code.
        /// </summary>
        public int Code { get; set; }

        public UserFriendlyException() { }

        public UserFriendlyException(string message)
            : base(message)
        {
        }

        public UserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UserFriendlyException(int code, string message)
            : this(message)
        {
            Code = code;
        }
    }
}