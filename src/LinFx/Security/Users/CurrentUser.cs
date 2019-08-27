using LinFx.Security.Claims;
using System;
using System.Linq;
using System.Security.Claims;
using ClaimTypes = LinFx.Security.Claims.ClaimTypes;

namespace LinFx.Security.Users
{
    [Service]
    public class CurrentUser : ICurrentUser
    {
        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);

        public virtual string Id => _principalAccessor.Principal?.FindUserId();

        public virtual string UserName => this.FindClaimValue(ClaimTypes.UserName);

        public virtual string PhoneNumber => this.FindClaimValue(ClaimTypes.PhoneNumber);

        public virtual bool PhoneNumberVerified => string.Equals(this.FindClaimValue(ClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public virtual string Email => this.FindClaimValue(ClaimTypes.Email);

        public virtual bool EmailVerified => string.Equals(this.FindClaimValue(ClaimTypes.EmailVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public virtual string TenantId => _principalAccessor.Principal?.FindTenantId();

        public virtual string[] Roles => FindClaims(ClaimTypes.Role).Select(c => c.Value).ToArray();

        private readonly IHttpContextPrincipalAccessor _principalAccessor;

        public CurrentUser(IHttpContextPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }

        public virtual Claim FindClaim(string claimType)
        {
            return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

        public virtual Claim[] GetAllClaims()
        {
            return _principalAccessor.Principal?.Claims.ToArray() ?? EmptyClaimsArray;
        }

        public bool IsInRole(string roleName)
        {
            return FindClaims(ClaimTypes.Role).Any(c => c.Value == roleName);
        }
    }
}
