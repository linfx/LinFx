using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperExtensionsServiceCollectionExtensions
    {
        public static ILinFxBuilder AddDapperExtensions(this ILinFxBuilder builder, Action setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            //builder.Services.Configure(setupAction);
            //builder.Services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());

            return builder;
        }
    }
}
