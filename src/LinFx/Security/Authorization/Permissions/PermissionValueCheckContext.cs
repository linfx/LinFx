using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace LinFx.Security.Authorization.Permissions
{
    public class PermissionValueCheckContext
    {
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