using Microsoft.AspNetCore.Authorization;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 权限策略
/// </summary>
public class PermissionRequirement(string permissionName) : IAuthorizationRequirement
{
    public string PermissionName { get; } = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
}
