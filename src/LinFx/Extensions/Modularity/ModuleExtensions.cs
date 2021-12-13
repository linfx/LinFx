using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ModuleExtensions
    {
        [Obsolete]
        public static IApplicationBuilder UseModules(this IApplicationBuilder app)
        {
            var moduleInitializers = app.ApplicationServices.GetServices<IModuleInitializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, null);
            }
            return app;
        }
    }
}
