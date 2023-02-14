using LinFx.Extensions.Data;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.MultiTenancy;

/// <summary>
/// 多租户模块
/// </summary>
[DependsOn(
    typeof(DataModule)
)]
public class MultiTenancyModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ICurrentTenant, CurrentTenant>();
        services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
    }
}
