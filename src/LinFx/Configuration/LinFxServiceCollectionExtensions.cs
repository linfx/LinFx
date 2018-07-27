using Microsoft.Extensions.DependencyInjection;
using System;
namespace LinFx
{
    public static class LinFxServiceCollectionExtensions
    {
        public static ILinFxBuilder AddLinFx(this IServiceCollection services)
        {
            var builder = new LinFxBuilder(services);
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
