using System.Security.Claims;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权服务
/// </summary>
public interface IAuthorizationService : Microsoft.AspNetCore.Authorization.IAuthorizationService
{
    ClaimsPrincipal CurrentPrincipal { get; }
}
