using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.PermissionManagement;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionManagementServiceCollectionExtensions
    {
        public static LinFxBuilder AddPermissionManagement(this LinFxBuilder builder)
        {
            builder.Services
                .AddSingleton<PermissionManager>()
                .AddSingleton<IPermissionChecker, PermissionChecker>()
                .AddTransient<IPermissionService, PermissionService>()
                .AddSingleton<IPermissionDefinitionContext, PermissionDefinitionContext>()
                .AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();

            return builder;
        }
    }
}
