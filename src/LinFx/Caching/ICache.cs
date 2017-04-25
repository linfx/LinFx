using System;
using System.Threading.Tasks;

namespace LinFx.Caching
{
    public interface ICache
    {
        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        Task<object> GetAsync(string key);

        /// <summary>
        /// Saves/Overrides an item in the cache by a key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expireTime">Expire time</param>
        Task SetAsync(string key, object value, TimeSpan? expireTime = default(TimeSpan?));
    }
}
