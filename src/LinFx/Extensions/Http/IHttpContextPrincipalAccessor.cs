using System.Security.Claims;

namespace LinFx.Security.Claims
{
    public interface IHttpContextPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}