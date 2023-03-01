using JetBrains.Annotations;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx;

public static class ServiceCollectionApplicationExtensions
{
    public static IApplicationWithExternalServiceProvider AddApplication<TStartupModule>([NotNull] this IServiceCollection services, [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IModule
    {
        return ApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static LinFxBuilder AddApplicationAsync<TStartupModule>([NotNull] this LinFxBuilder builder, [CanBeNull] Action<ApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IModule
    {
        builder.Services.AddApplication<TStartupModule>(optionsAction);
        return builder;
    }
}
