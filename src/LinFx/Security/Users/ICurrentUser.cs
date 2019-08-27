using System.Security.Claims;

namespace LinFx.Security.Users
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        [CanBeNull]
        string Id { get; }

        [CanBeNull]
        string UserName { get; }

        [CanBeNull]
        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        [CanBeNull]
        string Email { get; }

        bool EmailVerified { get; }

        string TenantId { get; }

        [NotNull]
        string[] Roles { get; }

        [CanBeNull]
        Claim FindClaim(string claimType);

        [NotNull]
        Claim[] FindClaims(string claimType);

        [NotNull]
        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
