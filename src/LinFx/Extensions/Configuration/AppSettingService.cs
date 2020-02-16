using LinFx.Extensions.Configuration.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Configuration
{
    public class AppSettingService : IAppSettingService
    {
        /// <summary>
        /// 默认缓存时间, 绝对过期时间 * 10
        /// </summary>
        private const int cacheTimeForSecond = 60;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public AppSettingService(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<string> GetAsync(string key)
        {
            return await GetByCahche<string>(key);
        }

        public async Task<T> GetAsync<T>()
        {
            return await GetByCahche<T>(typeof(T).Name);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await GetByCahche<T>(key);
        }

        public async Task ClearCacheAsync(string key)
        {
            _cache.Remove(key);
            await Task.CompletedTask;
        }

        async Task<T> GetByCahche<T>(string key)
        {
            return await _cache.GetOrCreateAsync(key, async (c) =>
            {
                c.SetSlidingExpiration(TimeSpan.FromSeconds(cacheTimeForSecond));
                c.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheTimeForSecond * 10));

                //var json = _configuration[key];
                //if (json == null)
                //    throw new ArgumentNullException(key);

                //var type = typeof(T);
                //if (type == typeof(string) || type == typeof(int))
                //    return (T)Convert.ChangeType(json, type);

                //var obj = json.ToObject<T>();
                //if (obj == null)
                //    throw new ArgumentNullException(key);

                var section = _configuration.GetSection(key);
                var result = section.Get<T>();

                return await Task.FromResult(result);
            });
        }
    }
}
