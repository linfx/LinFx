using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.Caching
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetAsync<T>(this ICache cache, string key, Func<Task<T>> func, TimeSpan? expiry = default(TimeSpan?))
        {
            var item = await cache.GetAsync<T>(key);
            if (item == null)
            {
                item = await func?.Invoke();
                await cache.SetAsync(key, item, expiry);
            }
            return item;
        }

        ///// <summary>
        ///// Asynchronously gets a string from the specified cache with the specified key.
        ///// </summary>
        ///// <param name="cache">The cache in which to store the data.</param>
        ///// <param name="key">The key to get the stored data for.</param>
        ///// <param name="func">Func</param>
        ///// <param name="options"></param>
        ///// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
        ///// <returns>A task that gets the string value from the stored cache key.</returns>
        //public static async Task<string> GetStringAsync(this IDistributedCache cache, string key, Func<Task<string>> func, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        //{
        //    var tmp = await func.Invoke();
        //    Func<Task<byte[]>> func2 =>  Encoding.UTF8.GetBytes(tmp);

        //    var data = await GetAsync(cache, key, func2, options, token);
        //    if (data == null)
        //    {
        //        return null;
        //    }
        //    return Encoding.UTF8.GetString(data, 0, data.Length); ;
        //}

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

    }
}
