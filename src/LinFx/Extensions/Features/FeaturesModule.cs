using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Features;

public class FeaturesModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.OnRegistered(FeatureInterceptorRegistrar.RegisterIfNeeded);
        AutoAddDefinitionProviders(services);

        services.Configure<FeatureOptions>(options =>
        {
            options.ValueProviders.Add<DefaultValueFeatureValueProvider>();
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IFeatureDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<FeatureOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}
