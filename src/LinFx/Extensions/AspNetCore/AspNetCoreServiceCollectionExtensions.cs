using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static LinFxBuilder AddAspNetCore(this LinFxBuilder builder)
        {
            builder.Services
                .AddHttpContextAccessor()
                .AddObjectAccessor<IApplicationBuilder>();

            return builder;
        }
    }
}
