using LinFx.Caching;
using LinFx.Caching.Memory;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.SaaS.UnitTest.Caching
{
    public class CacheTest
    {
        ICache _cache = new MemoryCache();

        [Fact]
        public async Task CreateAsyncTestAsync()
        {
            await _cache.SetAsync("A", 1);
            //await _cache.SetAsync("B")

            (await _cache.GetAsync("A")).ShouldBe(1);
        }
    }
}
