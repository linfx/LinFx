using JetBrains.Annotations;
using LinFx.Application;
using LinFx.Extensions.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionApplicationExtensions
{
    public static IApplicationWithExternalServiceProvider AddApplication<TStartupModule>([NotNull] this IServiceCollection services, [CanBeNull] Action<ApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IModule
    {
        return ApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static LinFxBuilder AddApplicationAsync<TStartupModule>([NotNull] this LinFxBuilder builder, [CanBeNull] Action<ApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IModule
    {
        AddApplication<TStartupModule>(builder.Services, optionsAction);
        return builder;
    }
}
