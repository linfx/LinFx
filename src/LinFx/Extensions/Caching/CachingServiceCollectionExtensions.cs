using LinFx.Extensions.Caching;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CachingServiceCollectionExtensions
    {
        public static LinFxBuilder AddCaching(this LinFxBuilder context)
        {
            context.Services.AddMemoryCache();
            context.Services.AddDistributedMemoryCache();

            context.Services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            context.Services.AddSingleton(typeof(IDistributedCache<,>), typeof(DistributedCache<,>));

            context.Services.Configure<DistributedCacheOptions>(cacheOptions =>
            {
                cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
            });

            return context;
        }
    }
}
