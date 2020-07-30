using System.Security.Claims;

namespace LinFx.Security.Authorization.Permissions
{
    /// <summary>
    /// 权限值检查上下文
    /// </summary>
    public class PermissionValueCheckContext
    {
        /// <summary>
        /// 权限
        /// </summary>
        public PermissionDefinition Permission { get; }

        /// <summary>
        /// 身份信息
        /// </summary>
        public ClaimsPrincipal Principal { get; }

        public PermissionValueCheckContext(PermissionDefinition permission, ClaimsPrincipal principal)
        {
            Permission = permission;
            Principal = principal;
        }
    }
}