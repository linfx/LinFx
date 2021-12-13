using System;

namespace LinFx.Extensions.Data;

public class DbConcurrencyException : Exception
{
    /// <summary>
    /// Creates a new <see cref="DbConcurrencyException"/> object.
    /// </summary>
    public DbConcurrencyException() { }

    /// <summary>
    /// Creates a new <see cref="DbConcurrencyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public DbConcurrencyException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new <see cref="DbConcurrencyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public DbConcurrencyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
