using LinFx.Extensions.Modularity;
using LinFx.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Data;

/// <summary>
/// 数据过滤模块
/// </summary>
[DependsOn(
    typeof(UnitOfWorkModule)
)]
public class DataModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        services.Configure<DbConnectionOptions>(configuration);
        services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
    }
}
