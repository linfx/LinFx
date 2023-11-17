using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement;

namespace LinFx.Extensions.TenantManagement.HttpApi;

[DependsOn(
    typeof(AspNetCoreMvcModule),
    typeof(PermissionManagementModule)
)]
public class PermissionManagementHttpApiModule : Module
{
}
