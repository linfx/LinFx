using Microsoft.AspNetCore.Authorization;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Authorization
{
    /// <summary>
    /// 权限策略
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement([NotNull] string permissionName)
        {
            PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
        }
    }
}
