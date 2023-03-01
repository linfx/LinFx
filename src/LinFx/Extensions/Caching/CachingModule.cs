using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Caching;

public class CachingModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();

        services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
        services.AddSingleton(typeof(IDistributedCache<,>), typeof(DistributedCache<,>));

        services.Configure<DistributedCacheOptions>(cacheOptions =>
        {
            cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
        });
    }
}
