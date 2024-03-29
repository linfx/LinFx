using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace LinFx.Security.Authorization;

/// <summary>
/// This exception is thrown on an unauthorized request.
/// </summary>
[Serializable]
public class AuthorizationException : Exception, IHasLogLevel, IHasErrorCode
{
    /// <summary>
    /// Severity of the exception.
    /// Default: Warn.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    /// <summary>
    /// Error code.
    /// </summary>
    public string Code { get; } = string.Empty;

    /// <summary>
    /// Creates a new <see cref="AuthorizationException"/> object.
    /// </summary>
    public AuthorizationException() { }

    /// <summary>
    /// Creates a new <see cref="AuthorizationException"/> object.
    /// </summary>
    public AuthorizationException(SerializationInfo serializationInfo, StreamingContext context) 
        : base(serializationInfo, context) 
    { }

    /// <summary>
    /// Creates a new <see cref="AuthorizationException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public AuthorizationException(string message)
        : base(message)
    {
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Creates a new <see cref="AuthorizationException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException)
    {
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Creates a new <see cref="AuthorizationException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="code">Exception code</param>
    /// <param name="innerException">Inner exception</param>
    public AuthorizationException(string message = null, string code = null, Exception innerException = null)
        : base(message, innerException)
    {
        Code = code;
        LogLevel = LogLevel.Warning;
    }

    public AuthorizationException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
