using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.MultiTenancy
{
    public abstract class HttpTenantResolveContributorBase : ITenantResolveContributor
    {
        public abstract string Name { get; }

        public virtual void Resolve(ITenantResolveContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            try
            {
                ResolveFromHttpContext(context, httpContext);
            }
            catch (Exception e)
            {
                context.ServiceProvider
                    .GetRequiredService<ILogger<HttpTenantResolveContributorBase>>()
                    .LogWarning(e.ToString());
            }
        }

        protected virtual void ResolveFromHttpContext(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantIdOrName = GetTenantIdOrNameFromHttpContextOrNull(context, httpContext);
            if (!string.IsNullOrEmpty(tenantIdOrName))
            {
                context.TenantIdOrName = tenantIdOrName;
            }
        }

        protected abstract string GetTenantIdOrNameFromHttpContextOrNull([NotNull] ITenantResolveContext context, [NotNull] HttpContext httpContext);
    }
}