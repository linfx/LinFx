using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement.Application;
using LinFx.Extensions.TenantManagement.Domain;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddTenantManagement(this LinFxBuilder builder, Action<MultiTenancyOptions> optionsAction = default)
        {
            builder.AddMultiTenancy(optionsAction);
            builder.Services.AddTransient<ITenantStore, TenantStore>();
            builder.Services.AddTransient<ITenantService, TenantService>();
            return builder;
        }
    }
}
