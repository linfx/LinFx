using LinFx.Extensions.Auditing;
using LinFx.Extensions.Caching;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 审计模块
/// </summary>
[DependsOn(
    typeof(CachingModule),
    typeof(AuditingModule),
    typeof(EntityFrameworkCoreModule)
)]
public class AuditLoggingModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
    }
}
