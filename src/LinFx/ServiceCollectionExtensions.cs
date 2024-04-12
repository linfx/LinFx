using System.Diagnostics.CodeAnalysis;
using LinFx;
using LinFx.Extensions.Modularity;
using LinFx.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 核心服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="application"></param>
    /// <param name="applicationCreationOptions"></param>
    internal static void AddCoreServices(this IServiceCollection services, IApplication application, ApplicationCreationOptions applicationCreationOptions)
    {
        services
            .AddOptions()
            .AddLocalization();

        var moduleLoader = new ModuleLoader();
        var assemblyFinder = new AssemblyFinder(application);
        var typeFinder = new TypeFinder(assemblyFinder);

        if (!services.IsAdded<IConfiguration>())
            services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(applicationCreationOptions.Configuration));

        services.TryAddSingleton<IModuleLoader>(moduleLoader);
        services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
        services.TryAddSingleton<ITypeFinder>(typeFinder);
        services.AddAssemblyOf<IApplication>();

        services.Configure<ModuleLifecycleOptions>(options =>
        {
            options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
        });
    }

    /// <summary>
    /// 增加应用
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithExternalServiceProvider AddApplication<TStartupModule>([NotNull] this IServiceCollection services, [AllowNull] Action<ApplicationCreationOptions>? optionsAction = null) where TStartupModule : IModule => ApplicationFactory.Create<TStartupModule>(services, optionsAction);
}
