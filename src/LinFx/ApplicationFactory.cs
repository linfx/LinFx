using System.Diagnostics.CodeAnalysis;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx;

/// <summary>
/// 应用创建工厂
/// </summary>
public class ApplicationFactory
{
    public static IApplicationWithExternalServiceProvider Create<TStartupModule>([NotNull] IServiceCollection services, [AllowNull] Action<ApplicationCreationOptions>? optionsAction = default) where TStartupModule : IModule => Create(typeof(TStartupModule), services, optionsAction);

    public static IApplicationWithInternalServiceProvider Create<TStartupModule>([AllowNull] Action<ApplicationCreationOptions>? optionsAction = default) where TStartupModule : IModule => Create(typeof(TStartupModule), optionsAction);

    private static IApplicationWithInternalServiceProvider Create([NotNull] Type startupModuleType, [AllowNull] Action<ApplicationCreationOptions>? optionsAction = default) => new ApplicationWithInternalServiceProvider(startupModuleType, optionsAction);

    private static IApplicationWithExternalServiceProvider Create([NotNull] Type startupModuleType, [NotNull] IServiceCollection services, [AllowNull] Action<ApplicationCreationOptions>? optionsAction = default) => new ApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
}
