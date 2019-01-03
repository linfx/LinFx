using LinFx;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LinFxServiceCollectionExtensions
    {
        public static ILinFxBuilder AddLinFx(this IServiceCollection services)
        {
            var builder = new LinFxBuilder(services);
            builder.Services.AddLogging();
            return builder;
        }

        public static ILinFxBuilder AddLinFx(this IServiceCollection services, Action<LinFxOptions> setupAction)
        {
            services.AddOptions();
            services.Configure(setupAction);
            return AddLinFx(services);
        }
    }
}
