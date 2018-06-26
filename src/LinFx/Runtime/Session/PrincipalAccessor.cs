using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Claims;
using System.Threading;

namespace LinFx.Session
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }

    public static class HttpContext
    {
        public static IPrincipalAccessor PrincipalAccessor { get; set; }
    }

    public class DefaultPrincipalAccessor : IPrincipalAccessor
    {
        public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;

        public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();
    }

    public class AspNetCorePrincipalAccessor : DefaultPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCorePrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
