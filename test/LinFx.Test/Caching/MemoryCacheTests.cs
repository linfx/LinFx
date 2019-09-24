using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.Caching
{
    public class MemoryCacheTest
    {
        [Fact]
        public async Task MemoryCache_GetAndSet_Tests()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddDistributedMemoryCache();

            var container = services.BuildServiceProvider();
            var cache = container.GetService<IDistributedCache>();

            Assert.NotNull(cache);

            var expected = 100;
            await cache.SetAsync("key1", 100);

            var actual = await cache.GetAsync<int>("key1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MemoryCache_GetOrAdd_Tests()
        {
            var services = new ServiceCollection();
            services
                .AddLinFx()
                .AddDistributedMemoryCache();

            var container = services.BuildServiceProvider();
            var cache = container.GetService<IDistributedCache>();


            var expected = new User
            {
                Id = 100
            };

            var actual = await cache.GetOrAddAsync("key2", () =>
            {
                return Task.FromResult(new User
                {
                    Id = 100
                });
            });

            Assert.Equal(expected, actual);
        }

        class User
        {
            public int Id { get; set; }

            public override bool Equals(object obj)
            {
                return obj.Equals(Id);
            }

            public override int GetHashCode()
            {
                return Id;
            }
        }
    }
}
