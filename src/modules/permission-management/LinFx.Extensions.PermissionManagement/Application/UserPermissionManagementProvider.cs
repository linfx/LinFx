using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement.Application;

[Service(ServiceLifetime.Scoped)]
public class UserPermissionManagementProvider(
    PermissionService permissionGrantRepository,
    IGuidGenerator guidGenerator,
    ICurrentTenant currentTenant) : PermissionManagementProvider(permissionGrantRepository, guidGenerator, currentTenant)
{
    public override string Name => UserPermissionValueProvider.ProviderName;
}
