using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Caching
{
    public interface ICache
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string key);
    }

    public static class CacheExtensions
    {
        public static async Task<T> GetAsync<T>(this ICache cache, string key, Func<Task<T>> factory, TimeSpan? expiry = default(TimeSpan?))
        {
            var item = await cache.GetAsync<T>(key);
            if (item == null)
            {
                item = await factory?.Invoke();
                await cache.SetAsync(key, item, expiry);
            }
            return item;
        }

        //public static Task<TValue> GetAsync<TValue>(this ICache cache, string key)
        //{
        //    return (TValue)cache.GetAsync(key);
        //}

        //public static async Task<TValue> GetAsync<TKey, TValue>(this ICache cache, TKey key, Func<TKey, Task<TValue>> factory)
        //{
        //    var value = await cache.GetAsync(key.ToString(), async (keyAsString) =>
        //    {
        //        return await factory(key);
        //    });
        //    return value;
        //}

        //public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, System.Func<T> acquire)
        //{
        //    if (cacheManager.IsSet(key))
        //        return cacheManager.Get<T>(key);
        //    var result = acquire();
        //    cacheManager.Set(key, result, cacheTime);
        //    return result;
        //}
    }
}
