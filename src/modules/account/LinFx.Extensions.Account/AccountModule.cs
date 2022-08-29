using LinFx.Extensions.Account;
using LinFx.Extensions.Account.Application;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccountModule
    {
        /// <summary>
        /// 账号模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static LinFxBuilder AddAccountExtensions(this LinFxBuilder services)
        {
            services.Services
                .AddDistributedMemoryCache()
                .AddSingleton<AuthenticationOptions>()
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IAccountService, AccountService>();

            return services;
        }
    }
}
