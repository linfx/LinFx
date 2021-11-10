using LinFx.Extensions.MultiTenancy;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyServiceCollectionExtensions
    {
        public static LinFxBuilder AddMultiTenancy(this LinFxBuilder builder, Action<MultiTenancyOptions> optionsAction = default)
        {
            if(optionsAction != null)
            {
                builder.Services.Configure(optionsAction);
            }

            builder.Services
                .AddTransient<ITenantResolver, TenantResolver>()
                .AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>()
                .AddTransient<ICurrentTenant, CurrentTenant>();

            return builder;
        }
    }
}
