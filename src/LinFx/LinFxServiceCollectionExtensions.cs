using LinFx.Application;
using LinFx.Extensions.Modularity;
using LinFx.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class LinFxServiceCollectionExtensions
{
    /// <summary>
    /// AddLinFx Code
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static LinFxBuilder AddLinFx(this IServiceCollection services, Action<LinFxOptions> optionsAction = default)
    {
        if (optionsAction != null)
            services.Configure(optionsAction);

        var builder = new LinFxBuilder(services);

        services.AddCoreServices();
        services.AddAssemblyOf<IApplication>();

        return builder;
    }

    /// <summary>
    /// 核心服务
    /// </summary>
    /// <param name="services"></param>
    internal static void AddCoreServices(this IServiceCollection services)
    {
        services
            .AddOptions()
            .AddLogging()
            .AddLocalization();
    }

    internal static void AddCoreLinFxServices(this IServiceCollection services, IApplication application, ApplicationCreationOptions applicationCreationOptions)
    {
        var moduleLoader = new ModuleLoader();
        var assemblyFinder = new AssemblyFinder(application);
        var typeFinder = new TypeFinder(assemblyFinder);

        if (!services.IsAdded<IConfiguration>())
        {
            //services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(applicationCreationOptions.Configuration));
        }

        services.TryAddSingleton<IModuleLoader>(moduleLoader);
        services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
        services.TryAddSingleton<ITypeFinder>(typeFinder);

        services.AddAssemblyOf<IApplication>();

        //services.AddTransient(typeof(ISimpleStateCheckerManager<>), typeof(SimpleStateCheckerManager<>));

        services.Configure<ModuleLifecycleOptions>(options =>
        {
            options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
        });
    }
}
