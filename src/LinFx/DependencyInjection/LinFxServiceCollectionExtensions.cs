using System;

namespace Microsoft.Extensions.DependencyInjection
{
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
            builder.Services.AddOptions();
            builder.Services.AddLogging();
            return builder;
        }
    }
}
