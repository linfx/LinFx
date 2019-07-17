using Microsoft.AspNetCore.Builder;

namespace LinFx.Extensions.MultiTenancy
{
    public static class MultiTenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MultiTenancyMiddleware>();
        }
    }
}
