using System.Security.Claims;

namespace LinFx.Extensions.Authorization.Permissions;

/// <summary>
/// Always allows for any permission.
/// Use IServiceCollection.AddAlwaysAllowAuthorization() to replace
/// IPermissionChecker with this class. This is useful for tests.
/// </summary>
public class AlwaysAllowPermissionChecker : IPermissionChecker
{
    /// <summary>
    /// 是否授权
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<PermissionGrantInfo> IsGrantedAsync(string name) => Task.FromResult(new PermissionGrantInfo(name, true, "AlwaysAllow"));

    /// <summary>
    /// 是否授权
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<PermissionGrantInfo> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name) => Task.FromResult(new PermissionGrantInfo(name, true, "AlwaysAllow"));
}
