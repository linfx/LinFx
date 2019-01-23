using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using ClaimTypes = LinFx.Security.Claims.ClaimTypes;

namespace LinFx.Security.Principal
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? FindUserId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.UserId);
            if (claim == null || claim.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            return Guid.Parse(claim.Value);
        }

        public static Guid? FindTenantId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.TenantId);
            if (claim == null || claim.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            return Guid.Parse(claim.Value);
        }

        public static Guid? FindClientId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.ClientId);
            if (claim == null || claim.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            return Guid.Parse(claim.Value);
        }

        public static Guid? FindUserId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(userIdOrNull.Value);
        }

        public static Guid? FindTenantId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.TenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(tenantIdOrNull.Value);
        }
    }
}
