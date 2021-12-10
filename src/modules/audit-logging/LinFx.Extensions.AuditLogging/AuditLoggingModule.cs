using LinFx.Extensions.Auditing;
using LinFx.Extensions.AuditLogging.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 审计模块
/// </summary>
public class AuditLoggingModule : Module
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<AuditingOptions>(options =>
        {
        });

        context.Services.AddDbContext<AuditLoggingDbContext>(options =>
        {
            options.AddRepository<AuditLog, EfCoreAuditLogRepository>();
        });

        context.Services.AddTransient<IAuditLoggingDbContext, AuditLoggingDbContext>();
    }
}
