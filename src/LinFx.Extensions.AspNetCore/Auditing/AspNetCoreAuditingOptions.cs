using System.Collections.Generic;

namespace LinFx.Extensions.AspNetCore.Auditing;

public class AspNetCoreAuditingOptions
{
    /// <summary>
    /// This is used to disable the <see cref="AuditingMiddleware"/>,
    /// app.UseAuditing(), for the specified URLs.
    /// <see cref="AuditingMiddleware"/> will be disabled for URLs
    /// starting with an ignored URL.  
    /// </summary>
    public List<string> IgnoredUrls { get; } = new List<string>();
}
