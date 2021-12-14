using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.Data;

/// <summary>
/// 数据过滤模块
/// </summary>
[DependsOn(
    typeof(UnitOfWorkModule)
)]
public class DataModule : Module
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDataSeedContributors(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<DbConnectionOptions>(configuration);

        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        //Configure<DbConnectionOptions>(options =>
        //{
        //    options.Databases.RefreshIndexes();
        //});
    }

    private static void AutoAddDataSeedContributors(IServiceCollection services)
    {
        var contributors = new List<Type>();

        services.OnRegistred(context =>
        {
            //if (typeof(IDataSeedContributor).IsAssignableFrom(context.ImplementationType))
            //{
            //    contributors.Add(context.ImplementationType);
            //}
        });

        //services.Configure<DataSeedOptions>(options =>
        //{
        //    options.Contributors.AddIfNotContains(contributors);
        //});
    }
}
