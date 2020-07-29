using System.Diagnostics.CodeAnalysis;
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
        [NotNull]
        public PermissionDefinition Permission { get; }

        public ClaimsPrincipal Principal { get; }

        public PermissionValueCheckContext([NotNull] PermissionDefinition permission, ClaimsPrincipal principal)
        {
            Check.NotNull(permission, nameof(permission));

            Permission = permission;
            Principal = principal;
        }
    }
}