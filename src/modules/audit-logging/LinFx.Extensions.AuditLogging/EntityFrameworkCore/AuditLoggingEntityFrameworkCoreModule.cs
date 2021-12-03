using LinFx.Extensions.Modularity;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore;

//[DependsOn(typeof(AbpAuditLoggingDomainModule))]
//[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class AuditLoggingEntityFrameworkCoreModule : Module
{
    //public override void ConfigureServices(ServiceConfigurationContext context)
    //{
    //    context.Services.AddAbpDbContext<AuditLoggingDbContext>(options =>
    //    {
    //        options.AddRepository<AuditLog, EfCoreAuditLogRepository>();
    //    });
    //}
}
