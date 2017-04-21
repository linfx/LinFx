using LinFx.Authorization.Accounts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.SaaS.UnitTest.Authorization.Accounts
{
    public class AccountManagerTest : UnitTestBase
    {
        AccountManager _accountManager;

        public AccountManagerTest()
        {
            UsingRepository<Account>(repository =>
            {
                _accountManager = new AccountManager(repository);
            });
        }

        [Fact]
        public void CreateAsyncTest()
        {
            var item = new Account
            {
                UserName = DateTime.Now.ToString(),
            };
            _accountManager.CreateAsync(item);
        }

        [Fact]
        public async Task UpdateAsyncTestAsync()
        {
            var item = await _accountManager.GetAsync("6d172b0ba4a9497b9053624aa7153b15");
            Assert.NotNull(item);

            item.UserName = "ok";
            await _accountManager.UpdateAsync(item);
        }

        [Fact]
        public async Task GetAsyncTestAsync()
        {
            var item = await _accountManager.GetAsync("6d172b0ba4a9497b9053624aa7153b15");
            Assert.NotNull(item);
        }
    }
}
