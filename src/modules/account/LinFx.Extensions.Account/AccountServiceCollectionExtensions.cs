using LinFx.Extensions.Account;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccountServiceCollectionExtensions
    {
        /// <summary>
        /// 账号模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static LinFxBuilder AddAccountExtensions(this LinFxBuilder services)
        {
            services.Services
                //.AddTransient<ITokenService, TokenService>()
                .AddTransient<IAccountService, AccountService>();

            return services;
        }
    }
}
