using LinFx.Extensions.Auditing;
using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore.Uow;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.EntityFrameworkCore;

[DependsOn(
    typeof(AuditingModule),
    typeof(DataModule),
    //typeof(EventBusModule),
    //typeof(AbpGuidsModule),
    typeof(MultiTenancyModule),
    typeof(ThreadingModule)
    //typeof(AbpTimingModule),
    //typeof(UnitOfWorkModule),
    //typeof(AbpObjectMappingModule),
    //typeof(AbpExceptionHandlingModule),
    //typeof(AbpSpecificationsModule)
)]
public class EntityFrameworkCoreModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<EfCoreDbContextOptions>(options =>
        {
            options.PreConfigure(dbContextConfigurationContext =>
            {
                dbContextConfigurationContext.DbContextOptions.ConfigureWarnings(warnings =>
                {
                    //warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
                });
            });
        });

        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
    }
}
