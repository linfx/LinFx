using LinFx.Extensions.Authorization.Permissions;
using TenantManagementService.Host.Permissions;

namespace Microsoft.Extensions.DependencyInjection;

public static class BloggingServiceCollectionExtensions
{
    public static LinFxBuilder AddBlogging(this LinFxBuilder builder)
    {
        builder
            .AddAssembly(typeof(BloggingServiceCollectionExtensions).Assembly);

        builder.Services.Configure<PermissionOptions>(options =>
        {
            options.DefinitionProviders.Add(typeof(BloggingPermissionDefinitionProvider));
        });

        return builder;
    }
}
