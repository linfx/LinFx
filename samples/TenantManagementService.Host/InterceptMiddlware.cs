using LinFx.Extensions.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TenantManagementService.Host
{
    public class InterceptMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IUnitOfWork now;

        public InterceptMiddlware(RequestDelegate next, IUnitOfWorkManager unitOfWorkManager)
        {
            _next = next;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Invoke(HttpContext context)
        {
            PreProceed(context);
            await _next(context);
            PostProceed(context);
        }

        private void PreProceed(HttpContext context)
        {
            now = _unitOfWorkManager.Begin();
        }

        private void PostProceed(HttpContext context)
        {
            now.CompleteAsync();
        }
    }

    public static class InterceptHandler
    {
        public static IApplicationBuilder UseInterceptMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<InterceptMiddlware>();
        }
    }
}
