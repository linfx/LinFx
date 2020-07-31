using System.Security.Claims;

namespace LinFx.Extensions.Authorization
{
    /// <summary>
    /// Checks policy based permissions for a user
    /// </summary>
    public interface IAuthorizationService : Microsoft.AspNetCore.Authorization.IAuthorizationService
    {
        ClaimsPrincipal CurrentPrincipal { get; }
    }
}
