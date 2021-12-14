using LinFx.Extensions.Data;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计模块
/// </summary>
[DependsOn(
    typeof(DataModule),
    //typeof(AbpTimingModule),
    //typeof(AbpSecurityModule),
    typeof(ThreadingModule),
    typeof(MultiTenancyModule)
    //typeof(AbpAuditingContractsModule)
)]
public class AuditingModule : Module
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(AuditingInterceptorRegistrar.RegisterIfNeeded);
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IAuditingStore, SimpleLogAuditingStore>()
            .AddTransient<IAuditingFactory, AuditingFactory>()
            .AddTransient<IAuditingManager, AuditingManager>();
    }
}
