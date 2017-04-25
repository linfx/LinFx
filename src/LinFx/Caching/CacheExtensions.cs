using System;
using System.Threading.Tasks;

namespace LinFx.Caching
{
    public static class CacheExtensions
    {
        public static async Task<object> GetAsync(this ICache cache, string key, Func<string, Task<object>> factory)
        {
            var cacheKey = key;
            var item = await cache.GetAsync(key);
            if (item == null)
            {
                await factory(key);
                await cache.SetAsync(cacheKey, item);
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
        //    return (TValue)value;
        //}
    }
}
