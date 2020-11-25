using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddPermissionManagement(this LinFxBuilder builder)
        {
            builder.Services.AddTransient<IPermissionService, PermissionService>();
            builder.Services.AddSingleton<PermissionManager>();

            //services.AddHttpContextAccessor();
            //services.AddTransient<IHttpContextPrincipalAccessor, HttpContextPrincipalAccessor>();

            //services.AddSingleton<IPermissionChecker, PermissionChecker>();
            builder.Services.AddSingleton<IPermissionDefinitionContext, PermissionDefinitionContext>();
            builder.Services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();

            return builder;
        }
    }
}
