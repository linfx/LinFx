using System.Security.Claims;

namespace Microsoft.AspNetCore.Http
{
    public interface IHttpContextPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
