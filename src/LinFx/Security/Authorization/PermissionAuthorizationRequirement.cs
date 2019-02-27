using Microsoft.AspNetCore.Authorization;

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
