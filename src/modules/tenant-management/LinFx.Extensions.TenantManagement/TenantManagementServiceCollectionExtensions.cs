using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.TenantManagement;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddTenantManagement(this LinFxBuilder builder, Action<MultiTenancyOptions> optionsAction = default)
        {
            builder
                .AddAssembly(typeof(TenantManagementServiceCollectionExtensions).Assembly);

            builder.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add(typeof(TenantManagementPermissionDefinitionProvider));
            });

            return builder;
        }
    }
}
