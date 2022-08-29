﻿using LinFx.Extensions.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinFx.Extensions.AspNetCore.MultiTenancy
{
    public class HeaderTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Header";

        public override string Name => ContributorName;

        protected override string? GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request == null || httpContext.Request.Headers.IsNullOrEmpty())
                return null;

            var tenantIdKey = context.GetMultiTenancyOptions().TenantKey;
            var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
            if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
                return null;

            if (tenantIdHeader.Count > 1)
                Log(context, $"HTTP request includes more than one {tenantIdKey} header value. First one will be used. All of them: {tenantIdHeader.JoinAsString(", ")}");

            return tenantIdHeader.First();
        }

        protected virtual void Log(ITenantResolveContext context, string text)
        {
            context
                .ServiceProvider
                .GetRequiredService<ILogger<HeaderTenantResolveContributor>>()
                .LogWarning(text);
        }
    }
}