using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace LinFx.Security.Claims;

public static class ClaimsIdentityExtensions
{
    public static string? FindUserId([NotNull] this ClaimsPrincipal principal)
    {
        Check.NotNull(principal, nameof(principal));

        var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Id || c.Type == JwtClaimTypes.Id || c.Type == JwtClaimTypes.Subject);
        if (claim == null || claim.Value.IsNullOrWhiteSpace())
            return default;

        return claim.Value;
    }

    public static string? FindUserId([NotNull] this IIdentity identity)
    {
        Check.NotNull(identity, nameof(identity));

        var claimsIdentity = identity as ClaimsIdentity;
        var claim = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Id);
        if (claim == null || claim.Value.IsNullOrWhiteSpace())
            return default;

        return claim.Value;
    }

    public static string? FindTenantId([NotNull] this ClaimsPrincipal principal)
    {
        Check.NotNull(principal, nameof(principal));

        var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.TenantId);
        if (claim == null || claim.Value.IsNullOrWhiteSpace())
            return default;

        return claim.Value;
    }

    public static string? FindTenantId([NotNull] this IIdentity identity)
    {
        Check.NotNull(identity, nameof(identity));

        var claimsIdentity = identity as ClaimsIdentity;
        var claim = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.TenantId);
        if (claim == null || claim.Value.IsNullOrWhiteSpace())
            return default;

        return claim.Value;
    }

    public static string? FindClientId([NotNull] this ClaimsPrincipal principal)
    {
        Check.NotNull(principal, nameof(principal));

        var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.ClientId);
        if (claim == null || claim.Value.IsNullOrWhiteSpace())
            return default;

        return claim.Value;
    }

    public static string? FindEditionId([NotNull] this ClaimsPrincipal principal)
    {
        Check.NotNull(principal, nameof(principal));

        var claim = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.EditionId);
        if (claim == null || claim.Value.IsNullOrWhiteSpace())
            return null;

        return claim.Value;
    }
}
