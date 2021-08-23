using LinFx.Test.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.Caching
{
    public class MemoryCacheTest
    {
        private readonly IDistributedCache _cache;

        public MemoryCacheTest()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddDistributedMemoryCache();

            var container = services.BuildServiceProvider();
            _cache = container.GetService<IDistributedCache>();
        }

        [Fact]
        public async Task MemoryCache_GetAndSet_Tests()
        {
            var expected = 100;
            await _cache.SetAsync("key1", 100);

            var actual = await _cache.GetAsync<int>("key1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MemoryCache_GetOrAdd_Tests()
        {
            var expected = new User
            {
                Id = 100
            };

            var actual = await _cache.GetOrAddAsync("key2", () =>
            {
                return Task.FromResult(new User
                {
                    Id = 100
                });
            });

            Assert.Equal(expected, actual);
        }
    }
}
