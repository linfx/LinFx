using LinFx;
using LinFx.Utils;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching
{
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="cache"><see cref="IDistributedCache"/></param>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<TCacheItem> GetAsync<TCacheItem>(this IDistributedCache cache, 
            [NotNull] string key, 
            CancellationToken token = default)
        {
            var value = await cache.GetAsync(key, token);
            if (value != null)
                return JsonUtils.ToObject<TCacheItem>(value); ;

            return default;
        }

        /// <summary>
        /// Gets a value or add a value with the given key.
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="cache"><see cref="IDistributedCache"/></param>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="factory"></param>
        /// <param name="optionsFactory"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<TCacheItem> GetOrAddAsync<TCacheItem>(this IDistributedCache cache,
            [NotNull] string key,
            Func<Task<TCacheItem>> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            CancellationToken token = default)
        {
            var value = await cache.GetAsync<TCacheItem>(key, token);
            if (value != null)
            {
                return value;
            }

            await cache.SetAsync(key, JsonUtils.ToBytes(value), optionsFactory?.Invoke(), token);

            return value;
        }
    }
}
