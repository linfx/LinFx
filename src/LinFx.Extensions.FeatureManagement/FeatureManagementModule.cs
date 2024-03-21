using LinFx.Extensions.Features;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 特征管理模块
/// </summary>
[DependsOn(
    //typeof(CachingModule),
    typeof(FeaturesModule)
)]
public class FeatureManagementModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        //services.Configure<PermissionManagementOptions>(options =>
        //{
        //    options.ManagementProviders.Add<UserPermissionManagementProvider>();
        //    options.ManagementProviders.Add<RolePermissionManagementProvider>();

        //    //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
        //    options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "Users.ManagePermissions";
        //    options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "Roles.ManagePermissions";
        //});
    }
}
