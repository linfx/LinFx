using System.Security.Claims;

namespace LinFx.Extensions.Authorization.Permissions;

public static class PermissionCheckerExtensions
{
    public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, string name) => (await permissionChecker.IsGrantedAsync(name)).IsGranted;

    public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, ClaimsPrincipal principal, string name) => (await permissionChecker.IsGrantedAsync(principal, name)).IsGranted;
}
