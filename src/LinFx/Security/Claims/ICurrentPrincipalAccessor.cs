using System.Security.Claims;

namespace LinFx.Security.Claims;

public interface ICurrentPrincipalAccessor
{
    /// <summary>
    /// ClaimsPrincipal（证件持有者）
    /// 一个 ClaimsIdentity 可以包含多个的 Claim，而一个 ClaimsPrincipal 可以包含多个的 ClaimsIdentity。
    /// </summary>
    ClaimsPrincipal Principal { get; }
}