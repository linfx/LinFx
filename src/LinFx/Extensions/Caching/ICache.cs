using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Caching
{
    public interface ICache : IDistributedCache
    {
        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expiry">Expire time</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?));
    }
}
