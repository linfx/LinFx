using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块
/// </summary>
public interface IModule
{
    void ConfigureServices(IServiceCollection services);
}