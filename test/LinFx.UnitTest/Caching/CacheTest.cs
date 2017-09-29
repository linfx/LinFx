using LinFx.Caching;
using LinFx.Caching.Memory;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.UnitTest.Caching
{
    public class CacheTest
    {
        ICache _cache = new MemoryCache();

        [Fact]
        public async Task CreateAsyncTestAsync()
        {
            await _cache.SetAsync("A", 1);
            //await _cache.SetAsync("B")

            (await _cache.GetAsync<int>("A")).ShouldBe(1);
        }
    }
}
