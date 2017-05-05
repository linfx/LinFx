using LinFx.SaaS.Editions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.SaaS.UnitTest.Editions
{
    public class EditionManagerTest : UnitTestBase
    {
        EditionManager _editionManager;

        public EditionManagerTest()
        {
            UsingRepository<Edition>(repository =>
            {
                _editionManager = new EditionManager(repository);
            });
        }

        [Fact]
        public async Task CreateAsyncTestAsync()
        {
            var item = new Edition
            {
                Name = Guid.NewGuid().ToString("N"),
            };
            await _editionManager.CreateAsync(item);

            (await _editionManager.GetAsync(item.Id)).ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateAsyncTestAsync()
        {
            var item = await _editionManager.GetAsync("50d5f7cf0828454c88969fe9f82eea77");

            item.ShouldNotBeNull();

            item.Name = "ok";
            await _editionManager.UpdateAsync(item);
        }

        [Fact]
        public async Task GetAsyncTestAsync()
        {
            var item = await _editionManager.GetAsync("50d5f7cf0828454c88969fe9f82eea77");
            Assert.NotNull(item);
        }
    }
}
