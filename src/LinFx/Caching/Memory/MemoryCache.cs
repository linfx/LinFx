using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Caching.Memory
{
    public class MemoryCache : CacheBase
    {
        Microsoft.Extensions.Caching.Memory.MemoryCache _cache;

        public MemoryCache()
        {
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public override Task<object> GetAsync(string key)
        {
            return Task.FromResult(_cache.Get(key));
        }

        public override Task SetAsync(string key, object value, TimeSpan? expireTime = default(TimeSpan?))
        {
            if (value == null)
            {
                throw new Exception(nameof(value));
            }

            if(expireTime != null)
            {
                return Task.FromResult(_cache.Set(key, value, DateTimeOffset.Now.Add(expireTime.Value)));
            }
            else
            {
                return Task.FromResult(_cache.Set(key, value, TimeSpan.FromHours(1)));
            }
        }
    }
}
