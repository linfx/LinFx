using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissionManagement(this IServiceCollection services)
        {
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddSingleton<PermissionManager>();

            //services.AddHttpContextAccessor();
            //services.AddTransient<IHttpContextPrincipalAccessor, HttpContextPrincipalAccessor>();

            //services.AddSingleton<IPermissionChecker, PermissionChecker>();
            services.AddSingleton<IPermissionDefinitionContext, PermissionDefinitionContext>();
            services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();

            return services;
        }
    }
}
