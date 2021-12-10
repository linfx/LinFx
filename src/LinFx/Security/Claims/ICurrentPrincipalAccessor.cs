using System.Security.Claims;

namespace LinFx.Security.Claims
{
    public interface ICurrentPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}