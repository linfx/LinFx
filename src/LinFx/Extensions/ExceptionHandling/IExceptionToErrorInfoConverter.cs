﻿using LinFx.Extensions.Http;

namespace LinFx.Extensions.ExceptionHandling;

/// <summary>
/// This interface can be implemented to convert an <see cref="Exception"/> object to an <see cref="RemoteServiceErrorInfo"/> object.
/// Implements Chain Of Responsibility pattern.
/// </summary>
public interface IExceptionToErrorInfoConverter
{
    /// <summary>
    /// Converter method.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="options">Additional options.</param>
    /// <returns>Error info or null</returns>
    RemoteServiceErrorInfo Convert(Exception exception, Action<ExceptionHandlingOptions>? options = null);
}
