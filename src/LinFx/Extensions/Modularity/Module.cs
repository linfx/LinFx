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
public abstract class Module : IModuleInitializer, IModule
{
    private ServiceConfigurationContext _serviceConfigurationContext;

    protected internal bool SkipAutoServiceRegistration { get; protected set; }

    protected internal ServiceConfigurationContext ServiceConfigurationContext
    {
        get
        {
            if (_serviceConfigurationContext == null)
                throw new LinFxException($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PostConfigureServices)} methods.");

            return _serviceConfigurationContext;
        }
        internal set => _serviceConfigurationContext = value;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
    }

    public virtual void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
    }

    public virtual void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureServices(context.Services);
    }

    public virtual void PostConfigureServices(ServiceConfigurationContext context)
    {
    }

    public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        Configure(app, env);
    }

    protected void Configure<TOptions>(IConfiguration configuration)
        where TOptions : class
    {
        ServiceConfigurationContext.Services.Configure<TOptions>(configuration);
    }
}
