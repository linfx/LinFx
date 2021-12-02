using LinFx.Extensions.Modules;
using System;

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

        builder
            .AddAssembly(typeof(Module).Assembly);

        builder.Services
            .AddOptions()
            .AddLogging();

        return builder;
    }
}
