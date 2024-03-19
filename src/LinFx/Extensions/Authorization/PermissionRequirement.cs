using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 权限策略
/// </summary>
public class PermissionRequirement([NotNull] string permissionName) : IAuthorizationRequirement
{
    public string PermissionName { get; } = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
}
