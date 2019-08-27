using Microsoft.Extensions.Caching.Redis;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up Redis distributed cache related services in an <see cref="LinFxBuilder" />.
    /// </summary>
    public static class RedisCacheServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Redis distributed caching services to the specified <see cref="LinFxBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="LinFxBuilder" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{RedisCacheOptions}"/> to configure the provided
        /// <see cref="RedisCacheOptions"/>.</param>
        /// <returns>The <see cref="LinFxBuilder"/> so that additional calls can be chained.</returns>
        public static LinFxBuilder AddDistributedRedisCache(this LinFxBuilder builder, Action<RedisCacheOptions> setupAction)
        {
            builder.Services.AddDistributedRedisCache(setupAction);
            return builder;
        }
    }
}
