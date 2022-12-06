using LinFx.Extensions.AspNetCore.Auditing;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="AuditingMiddleware"/> to the specified Microsoft.AspNetCore.Builder.IApplicationBuilder.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAuditing(this IApplicationBuilder app) => app.UseMiddleware<AuditingMiddleware>();
}
