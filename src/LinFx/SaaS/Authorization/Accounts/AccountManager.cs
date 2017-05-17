using LinFx.Data;
using LinFx.Domain.Services;

namespace LinFx.Authorization.Accounts
{
    public class AccountManager : DomainService<Account>
    {
        public AccountManager(IRepository<Account> repository) : base(repository) { }
    }
}