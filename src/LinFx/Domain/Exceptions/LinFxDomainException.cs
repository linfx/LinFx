using System;

namespace LinFx.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class LinFxDomainException : Exception
    {
        public LinFxDomainException()
        { }

        public LinFxDomainException(string message)
            : base(message)
        { }

        public LinFxDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
