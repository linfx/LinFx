using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore.MultiTenancy;

/// <summary>
/// 多租户模块
/// </summary>
[DependsOn(
    typeof(AspNetCoreModule),
    typeof(MultiTenancyModule)
)]
public class AspNetCoreMultiTenancyModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TenantResolveOptions>(options =>
        {
            //options.TenantResolvers.Add(new QueryStringTenantResolveContributor());
            //options.TenantResolvers.Add(new FormTenantResolveContributor());
            //options.TenantResolvers.Add(new RouteTenantResolveContributor());
            options.TenantResolvers.Add(new HeaderTenantResolveContributor());
        });
    }
}