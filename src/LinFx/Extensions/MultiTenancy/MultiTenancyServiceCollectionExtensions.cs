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

            builder.Services.AddTransient<ITenantResolver, TenantResolver>();
            builder.Services.AddTransient<ICurrentTenant, CurrentTenant>();
            builder.Services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            builder.Services.AddSingleton<ITenantResolveResultAccessor, HttpContextTenantResolveResultAccessor>();
            return builder;
        }
    }
}
