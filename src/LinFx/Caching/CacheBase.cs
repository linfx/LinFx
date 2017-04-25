using System;
using System.Threading.Tasks;

namespace LinFx.Caching
{
    public abstract class CacheBase : ICache
    {
        public abstract Task<object> GetAsync(string key);

        public abstract Task SetAsync(string key, object value, TimeSpan? expireTime = default(TimeSpan?));
    }
}
