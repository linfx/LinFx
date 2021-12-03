using LinFx.Extensions.Auditing;
using LinFx.Extensions.AuditLogging;
using LinFx.Extensions.AuditLogging.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuditLoggingServiceCollectionExtensions
{
    /// <summary>
    /// 审计模块
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static LinFxBuilder AddAuditLogging(this LinFxBuilder builder, Action<AuditingOptions> optionsAction = default)
    {
        builder
            .AddAssembly(typeof(AuditLoggingServiceCollectionExtensions).Assembly)
            .AddDbContext<AuditLoggingDbContext>(options =>
            {
                options.AddRepository<AuditLog, EfCoreAuditLogRepository>();
            });

        builder.Services.Configure<AuditingOptions>(options =>
        {
        });

        return builder;
    }
}
