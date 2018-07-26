using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace LinFx.Extensions.Caching.Redis
{
    public static class RedisCacheExtensions
    {
        public static Task<long> StringIncrementAsync(this IDistributedCache cache, string key, long value = 1)
        {
            return cache.As<RedisCache>().StringIncrementAsync(key, value);
        }

        public static Task<long> HashDecrementAsync(this IDistributedCache cache, string key, string hashField, long value = 1)
        {
            return cache.As<RedisCache>().HashDecrementAsync(key, hashField, value);
        }
    }
}
