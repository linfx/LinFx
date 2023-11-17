using LinFx.Extensions.DependencyInjection;
using LinFx.Security.Claims;
using System.Security.Claims;
using ClaimTypes = LinFx.Security.Claims.ClaimTypes;

namespace LinFx.Security.Users;

/// <summary>
/// 当前用户
/// </summary>
public class CurrentUser : ICurrentUser, ITransientDependency
{
    private static readonly Claim[] EmptyClaimsArray = new Claim[0];
    private readonly ICurrentPrincipalAccessor _principalAccessor;

    public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }

    public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);

    public virtual string Id => _principalAccessor.Principal?.FindUserId();

    public virtual string UserName => this.FindClaimValue(ClaimTypes.UserName);

    public virtual string PhoneNumber => this.FindClaimValue(ClaimTypes.PhoneNumber);

    public virtual bool PhoneNumberVerified => string.Equals(this.FindClaimValue(ClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);

    public virtual string Email => this.FindClaimValue(ClaimTypes.Email);

    public virtual bool EmailVerified => string.Equals(this.FindClaimValue(ClaimTypes.EmailVerified), "true", StringComparison.InvariantCultureIgnoreCase);

    public virtual string TenantId => _principalAccessor.Principal?.FindTenantId();

    public virtual string[] Roles => FindClaims(ClaimTypes.Role).Select(c => c.Value).ToArray();

    public virtual Claim FindClaim(string claimType) => _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);

    public virtual Claim[] FindClaims(string claimType) => _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;

    public virtual Claim[] GetAllClaims() => _principalAccessor.Principal?.Claims.ToArray() ?? EmptyClaimsArray;

    public bool IsInRole(string roleName) => FindClaims(ClaimTypes.Role).Any(c => c.Value == roleName);
}
