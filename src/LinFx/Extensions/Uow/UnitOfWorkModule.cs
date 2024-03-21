using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元模块
/// </summary>
public class UnitOfWorkModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.OnRegistered(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
    }
}
