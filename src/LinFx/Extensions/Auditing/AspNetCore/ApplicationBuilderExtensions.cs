using LinFx.Extensions.Auditing.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuditingMiddleware>();
    }
}
