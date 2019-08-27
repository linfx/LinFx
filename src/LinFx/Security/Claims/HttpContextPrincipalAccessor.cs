using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LinFx.Security.Claims
{
    [Service]
    public class HttpContextPrincipalAccessor : ThreadCurrentPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
