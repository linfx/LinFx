using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

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