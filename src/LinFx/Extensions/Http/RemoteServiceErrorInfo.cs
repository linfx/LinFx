using System.Collections;

namespace LinFx.Extensions.Http;

/// <summary>
/// Used to store information about an error.
/// </summary>
[Serializable]
public class RemoteServiceErrorInfo
{
    /// <summary>
    /// Error code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error details.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    public IDictionary? Data { get; set; }

    /// <summary>
    /// Validation errors if exists.
    /// </summary>
    public RemoteServiceValidationErrorInfo[]? ValidationErrors { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
    /// </summary>
    public RemoteServiceErrorInfo() { }

    /// <summary>
    /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="details">Error details</param>
    /// <param name="message">Error message</param>
    public RemoteServiceErrorInfo(string message, string details = default, string code = default)
    {
        Message = message;
        Details = details;
        Code = code;
    }
}
