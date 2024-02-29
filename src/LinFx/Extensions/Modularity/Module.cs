using LinFx.Extensions.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

    public virtual void Configure(IApplicationBuilder app, IHostEnvironment env) { }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        Configure(app, env);
    }

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
