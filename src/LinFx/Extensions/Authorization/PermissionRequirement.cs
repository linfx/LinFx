using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Authorization
{
    /// <summary>
    /// 定义权限 Requirement
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
