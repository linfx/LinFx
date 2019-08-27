using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.MultiTenancy
{
    public static class TenantResolveContextExtensions
    {
        public static MultiTenancyOptions GetMultiTenancyOptions(this ITenantResolveContext context)
        {
            return context.ServiceProvider.GetRequiredService<IOptionsSnapshot<MultiTenancyOptions>>().Value;
        }
    }
}