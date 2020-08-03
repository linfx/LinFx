using Microsoft.AspNetCore.Authorization;
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
            Check.NotNull(permissionName, nameof(permissionName));

            PermissionName = permissionName;
        }
    }
}
