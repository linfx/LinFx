using LinFx.Extensions.AspNetCore.MultiTenancy;

namespace Microsoft.AspNetCore.Builder;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MultiTenancyMiddleware>();
    }
}
