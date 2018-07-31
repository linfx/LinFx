using LinFx.Utils;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.Caching
{
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Asynchronously gets a string from the specified cache with the specified key.
        /// </summary>
        /// <param name="cache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <param name="func"></param>
        /// <param name="options"></param>
        /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        /// <returns>A task that gets the string value from the stored cache key.</returns>
        public static async Task<byte[]> GetAsync(this IDistributedCache cache, string key, Func<Task<byte[]>> func, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            var value = await cache.GetAsync(key, token);
            if (value == null)
            {
                value = await func.Invoke();
                await cache.SetAsync(key, value, options, token);
            }
            return value;
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> func, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            var tmp = await cache.GetAsync(key, token);
            if (tmp != null)
                return JsonUtils.ToObject<T>(tmp);

            var item = await func.Invoke();
            if (item == null)
                return default(T);

            await cache.SetAsync(key, JsonUtils.ToBytes(item), options, token);
            return item;
        }

    }
}
