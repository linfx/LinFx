using System.Security.Claims;

namespace Microsoft.AspNetCore.Http
{
    public class HttpContextPrincipalAccessor : IHttpContextPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public virtual ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User;

        public HttpContextPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}