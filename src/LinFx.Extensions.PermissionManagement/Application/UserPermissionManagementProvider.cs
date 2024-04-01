using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

[Service(ServiceLifetime.Scoped)]
public class UserPermissionManagementProvider(
    PermissionService permissionGrantRepository,
    ICurrentTenant currentTenant) : PermissionManagementProvider(permissionGrantRepository, currentTenant)
{
    public override string Name => UserPermissionValueProvider.ProviderName;
}
