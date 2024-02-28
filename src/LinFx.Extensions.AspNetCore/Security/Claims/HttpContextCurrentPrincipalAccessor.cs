using LinFx.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LinFx.Extensions.AspNetCore.Security.Claims;

public class HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor) : ThreadCurrentPrincipalAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;
}
