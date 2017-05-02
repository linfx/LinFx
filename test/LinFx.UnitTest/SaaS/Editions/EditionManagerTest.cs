using LinFx.SaaS.Editions;
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
        public void CreateAsyncTest()
        {
            var item = new Edition
            {
                Name = DateTime.Now.ToString(),
            };
            _editionManager.CreateAsync(item);
        }

        [Fact]
        public async Task UpdateAsyncTestAsync()
        {
            var item = await _editionManager.GetAsync("0a85e9c1fc944ea68b7860c6a7343ec2");
            Assert.NotNull(item);

            item.Name = "ok";
            await _editionManager.UpdateAsync(item);
        }

        [Fact]
        public async Task GetAsyncTestAsync()
        {
            var item = await _editionManager.GetAsync("0a85e9c1fc944ea68b7860c6a7343ec2");
            Assert.NotNull(item);
        }
    }
}
