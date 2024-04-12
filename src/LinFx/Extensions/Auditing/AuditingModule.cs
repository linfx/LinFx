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
    typeof(ThreadingModule),
    typeof(MultiTenancyModule)
)]
public class AuditingModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.OnRegistered(AuditingInterceptorRegistrar.RegisterIfNeeded);

        services
            .AddSingleton<IAuditingStore, SimpleLogAuditingStore>()
            .AddTransient<IAuditingFactory, AuditingFactory>()
            .AddTransient<IAuditingManager, AuditingManager>();
    }
}
