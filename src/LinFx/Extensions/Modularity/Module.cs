using LinFx.Application;
using LinFx.Extensions.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块
/// </summary>
public abstract class Module :
    IModuleInitializer,
    IModule,
    //IOnPreApplicationInitialization,
    IOnApplicationInitialization,
    //IOnPostApplicationInitialization,
    IOnApplicationShutdown,
    IPreConfigureServices,
    IPostConfigureServices
{
    private ServiceConfigurationContext? _serviceConfigurationContext;

    protected internal bool SkipAutoServiceRegistration { get; protected set; }

    protected internal ServiceConfigurationContext? ServiceConfigurationContext
    {
        get
        {
            if (_serviceConfigurationContext == null)
                throw new LinFxException($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PostConfigureServices)} methods.");

            return _serviceConfigurationContext;
        }
        internal set => _serviceConfigurationContext = value;
    }

    public virtual void ConfigureServices(IServiceCollection services) { }

    public virtual void Configure(IApplicationBuilder app, IHostEnvironment env) { }

    public virtual void PreConfigureServices(ServiceConfigurationContext context) { }

    public virtual void ConfigureServices(ServiceConfigurationContext context) => ConfigureServices(context.Services);

    public virtual void PostConfigureServices(ServiceConfigurationContext context) { }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public virtual Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        OnApplicationInitialization(context);
        return Task.CompletedTask;
    }

    public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        Configure(app, env);
    }

    /// <summary>
    /// 应用程序关闭
    /// </summary>
    /// <param name="context"></param>
    public virtual Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        return Task.CompletedTask;
    }

    protected void Configure<TOptions>(Action<TOptions> configureOptions)
        where TOptions : class
    {
        ServiceConfigurationContext?.Services.Configure(configureOptions);
    }

    protected void Configure<TOptions>(string name, Action<TOptions> configureOptions)
        where TOptions : class
    {
        ServiceConfigurationContext?.Services.Configure(name, configureOptions);
    }

    protected void Configure<TOptions>(IConfiguration configuration)
        where TOptions : class
    {
        ServiceConfigurationContext?.Services.Configure<TOptions>(configuration);
    }

    protected void Configure<TOptions>(IConfiguration configuration, Action<BinderOptions> configureBinder)
        where TOptions : class
    {
        ServiceConfigurationContext?.Services.Configure<TOptions>(configuration, configureBinder);
    }

    protected void Configure<TOptions>(string name, IConfiguration configuration)
        where TOptions : class
    {
        ServiceConfigurationContext?.Services.Configure<TOptions>(name, configuration);
    }
}
