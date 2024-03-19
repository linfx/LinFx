using LinFx.Extensions.Auditing;
using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 审计模块
/// </summary>
[DependsOn(
    typeof(DataModule),
    typeof(MultiTenancyModule),
    typeof(AuditingModule),
    typeof(EntityFrameworkCoreModule)
)]
public class AuditLoggingModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
    }
}
