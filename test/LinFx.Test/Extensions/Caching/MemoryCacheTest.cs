using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LinFx.Test.Caching
{
    public class MemoryCacheTest
    {
        [Fact]
        public void MemoryCacheGetAndSetTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var container = services.BuildServiceProvider();
            var _cache = container.GetService<IMemoryCache>();

            var expected = 100;
            _cache.Set("key1", expected);
            var actual = _cache.Get("key1");

            Assert.Equal(expected, actual);
        }
    }
}
