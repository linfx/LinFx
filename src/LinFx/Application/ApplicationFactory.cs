using JetBrains.Annotations;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Application;

public class ApplicationFactory
{
    public static IApplicationWithInternalServiceProvider Create<TStartupModule>(
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }

    public static IApplicationWithExternalServiceProvider Create<TStartupModule>(
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    private static IApplicationWithInternalServiceProvider Create(
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
    {
        return new ApplicationWithInternalServiceProvider(startupModuleType, optionsAction);
    }

    private static IApplicationWithExternalServiceProvider Create(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
    {
        return new ApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
    }
}
