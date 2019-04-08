using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LinFx.Test.Caching
{
    public class RedisCacheTest
    {
        private readonly IDistributedCache _cache;

        public RedisCacheTest()
        {
            var services = new ServiceCollection();
            services.AddLinFx()
                .AddDistributedRedisCache(options =>
                {
                    options.Configuration = "10.0.1.112:6379,password=redis";
                    options.InstanceName = "linfx_test:";
                });

            var container = services.BuildServiceProvider();
            _cache = container.GetService<IDistributedCache>();
        }

        [Fact]
        public void GetMissingKeyReturnsNull()
        {
            string key = "non-existent-key";
            var result = _cache.Get(key);
            Assert.Null(result);
        }

        [Fact]
        public void SetAndGetReturnsObject()
        {
            var value = new byte[1];
            string key = "myKey";
            _cache.Set(key, value);
            var result = _cache.Get(key);
            Assert.Equal(value, result);
        }

        [Fact]
        public void SetAndGetWorksWithCaseSensitiveKeys()
        {
            var value = new byte[1];
            string key1 = "myKey";
            string key2 = "Mykey";

            _cache.Set(key1, value);

            var result = _cache.Get(key1);
            Assert.Equal(value, result);

            result = _cache.Get(key2);
            Assert.Null(result);
        }
    }
}
