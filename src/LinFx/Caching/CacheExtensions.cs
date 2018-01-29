using System;
using System.Threading.Tasks;

namespace LinFx.Caching
{
    public static class CacheExtensions
    {
        public static async Task<T> GetAsync<T>(this ICache cache, string key, Func<string, Task<T>> factory, TimeSpan? expiry = default(TimeSpan?))
        {
            var cacheKey = key;
            var item = await cache.GetAsync<T>(key);
            if (item == null)
            {
                await factory(key);
                await cache.SetAsync(cacheKey, item, expiry);
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
