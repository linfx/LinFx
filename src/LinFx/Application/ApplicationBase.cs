using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Modularity;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Assembly = System.Reflection.Assembly;

namespace LinFx.Application;

public abstract class ApplicationBase : IApplication
{
    [NotNull]
    public Type StartupModuleType { get; }

    public IServiceProvider ServiceProvider { get; private set; }

    public IServiceCollection Services { get; }

    public IReadOnlyList<IModuleDescriptor> Modules { get; }

    internal ApplicationBase(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<ApplicationCreationOptions>? optionsAction)
    {
        Check.NotNull(startupModuleType, nameof(startupModuleType));
        Check.NotNull(services, nameof(services));

        // 设置启动模块。
        StartupModuleType = startupModuleType;
        Services = services;

        // 添加一个空的对象访问器，该访问器的值会在初始化的时候被赋值。
        services.TryAddObjectAccessor<IServiceProvider>();

        // 调用用户传入的配置委托。
        var options = new ApplicationCreationOptions(services);
        optionsAction?.Invoke(options);

        // 注册自己。
        services.AddSingleton<IApplication>(this);
        services.AddSingleton<IModuleContainer>(this);

        // 添加日志等基础设施组件。
        // 添加核心服务，主要是模块系统相关组件。
        services.AddCoreServices();
        services.AddCoreLinFxServices(this, options);

        Modules = LoadModules(services, options);
        ConfigureServices();
    }

    protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
    }

    /// <summary>
    /// 初始化模块
    /// </summary>
    /// <returns></returns>
    protected virtual async Task InitializeModulesAsync()
    {
        using var scope = ServiceProvider.CreateScope();
        WriteInitLogs(scope.ServiceProvider);
        await scope.ServiceProvider
            .GetRequiredService<IModuleManager>()
            .InitializeModulesAsync(new ApplicationInitializationContext(scope.ServiceProvider));
    }

    protected virtual void WriteInitLogs(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger<ApplicationBase>>();
        if (logger == null)
            return;

        //var initLogger = serviceProvider.GetRequiredService<IInitLoggerFactory>().Create<AbpApplicationBase>();

        //foreach (var entry in initLogger.Entries)
        //{
        //    logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
        //}

        //initLogger.Entries.Clear();
    }

    /// <summary>
    /// 加载模块
    /// </summary>
    protected virtual IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services, ApplicationCreationOptions options)
    {
        return services
            .GetSingletonInstance<IModuleLoader>()
            .LoadModules(services, StartupModuleType, options.PlugInSources);
    }

    //TODO: We can extract a new class for this
    public virtual void ConfigureServices()
    {
        var context = new ServiceConfigurationContext(Services);
        Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is Module item)
            {
                item.ServiceConfigurationContext = context;
            }
        }

        //PreConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
        {
            try
            {
                ((IPreConfigureServices)module.Instance).PreConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new LinFxException($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        var assemblies = new HashSet<Assembly>();

        //ConfigureServices
        foreach (var module in Modules)
        {
            if (module.Instance is Module item)
            {
                if (!item.SkipAutoServiceRegistration)
                {
                    var assembly = module.Type.Assembly;
                    if (!assemblies.Contains(assembly))
                    {
                        Services.AddAssembly(assembly);
                        assemblies.Add(assembly);
                    }
                }
            }

            try
            {
                module.Instance.ConfigureServices(context.Services);
            }
            catch (Exception ex)
            {
                throw new LinFxException($"An error occurred during {nameof(IModule.ConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        //PostConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
        {
            try
            {
                ((IPostConfigureServices)module.Instance).PostConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new LinFxException($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        foreach (var module in Modules)
        {
            if (module.Instance is Module item)
            {
                item.ServiceConfigurationContext = null;
            }
        }
    }

    public virtual async Task ShutdownAsync()
    {
        using var scope = ServiceProvider.CreateScope();
        await scope.ServiceProvider
            .GetRequiredService<IModuleManager>()
            .ShutdownModulesAsync(new ApplicationShutdownContext(scope.ServiceProvider));
    }

    public virtual void Dispose()
    {
    }
}
