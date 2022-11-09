using LinFx.Extensions.Account;
using LinFx.Extensions.Account.Application;
using LinFx.Extensions.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 账户模块
/// </summary>
public class AccountModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDistributedMemoryCache()
            .AddSingleton<AuthenticationOptions>()
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IAccountService, AccountService>();
    }
}
