using LinFx.Extensions.Caching;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.FeatureManagement;
using LinFx.Extensions.Features;
using LinFx.Extensions.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 特征管理模块
/// </summary>
[DependsOn(
    typeof(CachingModule),
    typeof(FeaturesModule),
    typeof(EntityFrameworkCoreModule)
)]
public class FeatureManagementModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<FeatureManagementOptions>(options =>
        {
            options.Providers.Add<DefaultValueFeatureManagementProvider>();
            options.Providers.Add<EditionFeatureManagementProvider>();
            options.Providers.Add<IdentityFeatureManagementProvider>();
        });
    }
}
