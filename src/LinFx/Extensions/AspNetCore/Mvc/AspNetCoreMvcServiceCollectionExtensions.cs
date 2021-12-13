using LinFx.Extensions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AspNetCoreMvcServiceCollectionExtensions
    {
        public static LinFxBuilder AddAspNetCoreMvc(this LinFxBuilder builder)
        {
            builder.AddAspNetCore();

            builder.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.AddOptions(builder.Services);
            });

            return builder;
        }
    }
}
