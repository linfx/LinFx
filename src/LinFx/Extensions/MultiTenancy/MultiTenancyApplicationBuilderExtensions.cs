using LinFx.Extensions.MultiTenancy;

namespace Microsoft.AspNetCore.Builder
{
    public static class MultiTenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MultiTenancyMiddleware>();
        }
    }
}
