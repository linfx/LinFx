using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionConfigurationExtensions
{
    public static IServiceCollection ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration) => services.Replace(ServiceDescriptor.Singleton(configuration));

    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
        if (hostBuilderContext?.Configuration != null)
            return (IConfigurationRoot)hostBuilderContext.Configuration;

        return services.GetSingletonInstance<IConfiguration>();
    }
}
