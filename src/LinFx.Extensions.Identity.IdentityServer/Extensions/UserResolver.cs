using Microsoft.AspNetCore.Identity;
using LinFx.Extensions.Identity.IdentityServer.Configuration;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.IdentityServer.Extensions
{
    public class UserResolver<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly LoginResolutionPolicy _policy;

        public UserResolver(UserManager<TUser> userManager, LoginConfiguration configuration)
        {
            _userManager = userManager;
            _policy = configuration.ResolutionPolicy;
        }

        public async Task<TUser> GetUserAsync(string login)
        {
            return _policy switch
            {
                LoginResolutionPolicy.Username => await _userManager.FindByNameAsync(login),
                LoginResolutionPolicy.Email => await _userManager.FindByEmailAsync(login),
                _ => null,
            };
        }
    }
}
