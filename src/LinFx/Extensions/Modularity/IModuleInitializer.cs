using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块初始化
/// </summary>
public interface IModuleInitializer
{
    void ConfigureServices(IServiceCollection services);
}
