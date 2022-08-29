using JetBrains.Annotations;
using LinFx;
using Microsoft.Net.Http.Headers;
using System;

namespace Microsoft.AspNetCore.Http;

public static class HttpRequestExtensions
{
    public static bool IsAjax([NotNull] this HttpRequest request)
    {
        Check.NotNull(request, nameof(request));

        return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
               string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    }

    public static bool CanAccept([NotNull] this HttpRequest request, [NotNull] string contentType)
    {
        Check.NotNull(request, nameof(request));
        Check.NotNull(contentType, nameof(contentType));

        return request.Headers[HeaderNames.Accept].ToString().Contains(contentType, StringComparison.OrdinalIgnoreCase);
    }
}
