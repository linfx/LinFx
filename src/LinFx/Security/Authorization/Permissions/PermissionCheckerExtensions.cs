using System.Security.Claims;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    public static class PermissionCheckerExtensions
    {
        public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, string name)
        {
            return (await permissionChecker.CheckAsync(name)).IsGranted;
        }

        public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, ClaimsPrincipal principal, string name)
        {
            return (await permissionChecker.CheckAsync(principal, name)).IsGranted;
        }
    }
}