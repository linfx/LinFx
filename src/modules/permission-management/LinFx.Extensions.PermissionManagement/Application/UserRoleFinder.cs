using LinFx.Extensions.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.PermissionManagement
{
    [Service(ServiceLifetime.Scoped)]
    public class UserRoleFinder : IUserRoleFinder
    {
        //protected IIdentityUserRepository IdentityUserRepository { get; }

        //public UserRoleFinder(IIdentityUserRepository identityUserRepository)
        //{
        //    IdentityUserRepository = identityUserRepository;
        //}

        public virtual Task<string[]> GetRolesAsync(string userId)
        {
            //return (await IdentityUserRepository.GetRoleNamesAsync(userId)).ToArray();
            return Task.FromResult(new string[] { });
        }
    }
}