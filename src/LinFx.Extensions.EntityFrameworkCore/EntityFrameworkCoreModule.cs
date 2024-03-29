using LinFx.Extensions.Auditing;
using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore.Uow;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.EntityFrameworkCore;

[DependsOn(
    typeof(DataModule),
    typeof(AuditingModule),
    typeof(EventBusModule),
    typeof(MultiTenancyModule),
    typeof(ThreadingModule),
    typeof(UnitOfWorkModule)
)]
public class EntityFrameworkCoreModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // 调用 DbContextOptions 的预配置方法，为了解决下面的问题。
        // https://stackoverflow.com/questions/55369146/eager-loading-include-with-using-uselazyloadingproxies
        services.Configure<EfDbContextOptions>(options =>
        {
            options.PreConfigure(dbContextConfigurationContext =>
            {
                dbContextConfigurationContext.DbContextOptions.ConfigureWarnings(warnings =>
                {
                    warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
                });
            });
        });

        // 注册 IDbContextProvider 组件。
        services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
    }
}
