using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Account.HttpApi;

[DependsOn(
    typeof(AccountModule)
)]
public class AccountHttpApiModule : Module
{
}
