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


            //item.Name = DateTime.Now.ToString() + DateTime.Now.ToString();

            //manager.UpdateAsync(item);

            //manager.DeleteAsync(item);
        }

        [Fact]
        public async Task GetByIdAsyncTestAsync()
        {
            var result = await _editionManager.GetByIdAsync("0a85e9c1fc944ea68b7860c6a7343ec2");

            Assert.NotNull(result);
        }
    }
}
