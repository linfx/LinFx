using LinFx;
using LinFx.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
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
                return JsonUtils.ToObject<TCacheItem>(value);

            return default;
        }

        /// <summary>
        /// Sets a avlue with the given key.
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="optionsFactory"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetAsync<TCacheItem>(this IDistributedCache cache,
            [NotNull] string key,
            TCacheItem item,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            CancellationToken token = default)
        {
            var options = new DistributedCacheEntryOptions();
            if (optionsFactory != null)
            {
                options = optionsFactory.Invoke();
            }

            return cache.SetAsync(key, JsonUtils.ToBytes(item), options, token);
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
            CancellationToken token = default) where TCacheItem : class
        {
            var value = await cache.GetAsync<TCacheItem>(key, token);
            if (value != null)
            {
                return value;
            }

            value = await factory.Invoke();
            var options = new DistributedCacheEntryOptions();
            if (optionsFactory != null)
            {
                options = optionsFactory.Invoke();
            }
            await cache.SetAsync(key, JsonUtils.ToBytes(value), options, token);
            return value;
        }
    }
}
