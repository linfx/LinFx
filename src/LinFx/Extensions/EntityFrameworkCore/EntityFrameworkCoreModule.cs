﻿using LinFx.Extensions.Auditing;
using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore.Uow;
using LinFx.Extensions.EventBus;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.EntityFrameworkCore;

[DependsOn(
    typeof(AuditingModule),
    typeof(DataModule),
    typeof(EventBusModule),
    //typeof(AbpGuidsModule),
    typeof(MultiTenancyModule),
    typeof(ThreadingModule),
    //typeof(AbpTimingModule),
    typeof(UnitOfWorkModule)
    //typeof(AbpObjectMappingModule),
    //typeof(AbpExceptionHandlingModule),
    //typeof(AbpSpecificationsModule)
)]
public class EntityFrameworkCoreModule : Module
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 调用 DbContextOptions 的预配置方法，为了解决下面的问题。
        // https://stackoverflow.com/questions/55369146/eager-loading-include-with-using-uselazyloadingproxies
        Configure<EfCoreDbContextOptions>(options =>
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
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
    }
}