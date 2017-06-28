using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LinFx.Session
{
    public class DefaultPrincipalAccessor : IPrincipalAccessor
    {
        public virtual ClaimsPrincipal Principal =>
#if NET46
            Thread.CurrentPrincipal as ClaimsPrincipal;
#else
            null;
#endif

        public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();
    }

    public class NetCorePrincipalAccessor : DefaultPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public NetCorePrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }

    public static class HttpContext
    {
        public static IPrincipalAccessor PrincipalAccessor { get; set; }
    }

    public static class PrincipalAccessorExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseNetCorePrincipalAccessor(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext.PrincipalAccessor = new NetCorePrincipalAccessor(httpContextAccessor);
            return app;
        }
    }
}
