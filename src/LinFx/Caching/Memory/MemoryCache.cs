using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LinFx.Caching.Memory
{
    public class MemoryCache : ICache
    {
        Microsoft.Extensions.Caching.Memory.MemoryCache _cache;

        public MemoryCache()
        {
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public Task<T> GetAsync<T>(string key)
        {
			return Task.FromResult(_cache.Get<T>(key));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expireTime = default(TimeSpan?))
        {
            if (value == null)
                throw new LinFxException(nameof(value));

            if(expireTime != null)
                return Task.FromResult(_cache.Set(key, value, DateTimeOffset.Now.Add(expireTime.Value)));
            else
                return Task.FromResult(_cache.Set(key, value, TimeSpan.FromHours(1)));
        }
    }
}