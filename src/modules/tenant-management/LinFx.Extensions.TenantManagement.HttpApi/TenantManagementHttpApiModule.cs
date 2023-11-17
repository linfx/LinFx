using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.Modularity;

namespace LinFx.Extensions.TenantManagement.HttpApi;

[DependsOn(
    typeof(AspNetCoreMvcModule),
    typeof(TenantManagementModule)
)]
public class TenantManagementHttpApiModule : Module
{
}
