using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement.Application;

[Service(ServiceLifetime.Scoped)]
public class UserPermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => UserPermissionValueProvider.ProviderName;

    public UserPermissionManagementProvider(
        PermissionService permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    { }
}
