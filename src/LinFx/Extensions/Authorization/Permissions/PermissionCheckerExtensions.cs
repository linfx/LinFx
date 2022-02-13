using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization.Permissions;

public static class PermissionCheckerExtensions
{
    public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, string name)
    {
        return (await permissionChecker.IsGrantedAsync(name)).IsGranted;
    }

    public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, ClaimsPrincipal principal, string name)
    {
        return (await permissionChecker.IsGrantedAsync(principal, name)).IsGranted;
    }
}
