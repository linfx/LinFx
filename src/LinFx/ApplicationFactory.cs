using JetBrains.Annotations;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx;

public class ApplicationFactory
{
    public static IApplicationWithExternalServiceProvider Create<TStartupModule>([NotNull] IServiceCollection services, [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null) where TStartupModule : IModule => Create(typeof(TStartupModule), services, optionsAction);

    public static IApplicationWithInternalServiceProvider Create<TStartupModule>([CanBeNull] Action<ApplicationCreationOptions> optionsAction = null) where TStartupModule : IModule => Create(typeof(TStartupModule), optionsAction);

    private static IApplicationWithInternalServiceProvider Create([NotNull] Type startupModuleType, [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null) => new ApplicationWithInternalServiceProvider(startupModuleType, optionsAction);

    private static IApplicationWithExternalServiceProvider Create([NotNull] Type startupModuleType, [NotNull] IServiceCollection services, [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null) => new ApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
}
