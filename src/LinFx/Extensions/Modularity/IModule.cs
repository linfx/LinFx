namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块
/// </summary>
public interface IModule
{
    void ConfigureServices(ServiceConfigurationContext context);
}