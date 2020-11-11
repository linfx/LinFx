using LinFx.Modules;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ModuleExtensions
    {
        public static void AddModules(ModuleOptions options)
        {
        }

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
