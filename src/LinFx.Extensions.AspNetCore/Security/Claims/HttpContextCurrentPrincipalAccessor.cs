using LinFx.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LinFx.Extensions.AspNetCore.Security.Claims;

public class HttpContextCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;
}
