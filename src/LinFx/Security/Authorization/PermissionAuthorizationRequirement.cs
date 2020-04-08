using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Security.Authorization
{
    /// <summary>
    /// 定义权限 Requirement
    /// </summary>
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionAuthorizationRequirement([NotNull]string permissionName)
        {
            Check.NotNull(permissionName, nameof(permissionName));

            PermissionName = permissionName;
        }
    }
}
