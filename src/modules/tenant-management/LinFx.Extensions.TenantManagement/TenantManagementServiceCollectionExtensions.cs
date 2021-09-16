using LinFx.Extensions.MultiTenancy;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddTenantManagement(this LinFxBuilder builder, Action<MultiTenancyOptions> optionsAction = default)
        {
            builder
                .AddAssembly(typeof(TenantManagementServiceCollectionExtensions).Assembly);

            return builder;
        }
    }
}
