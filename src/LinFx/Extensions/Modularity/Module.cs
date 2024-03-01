using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块
/// </summary>
public abstract class Module :
    IModuleInitializer,
    IModule,
    IOnApplicationInitialization,
    IOnApplicationShutdown
{
    protected internal bool SkipAutoServiceRegistration { get; protected set; }

    public virtual void ConfigureServices(IServiceCollection services) { }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public virtual void OnApplicationInitialization(ApplicationInitializationContext context) { }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public virtual Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnApplicationInitialization(context);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 应用程序关闭
    /// </summary>
    /// <param name="context"></param>
    public virtual Task OnApplicationShutdownAsync(ApplicationShutdownContext context) => Task.CompletedTask;
}
