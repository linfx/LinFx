using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.Modularity;

namespace LinFx.Extensions.TenantManagement.HttpApi;

[DependsOn(
    typeof(AspNetCoreMvcModule),
    typeof(TenantManagementModule)
)]
public class TenantManagementHttpApiModule : Module
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        //PreConfigure<IMvcBuilder>(mvcBuilder =>
        //{
        //    mvcBuilder.AddApplicationPartIfNotExists(typeof(TenantManagementHttpApiModule).Assembly);
        //});
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Configure<LocalizationOptions>(options =>
        //{
        //    options.Resources
        //        .Get<CmsKitResource>()
        //        .AddBaseTypes(typeof(AbpUiResource));
        //});
    }
}
