using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 管理用户基础权限
/// </summary>
/// <param name="permissionGrantRepository"></param>
/// <param name="currentTenant"></param>
[Service(ServiceLifetime.Singleton)]
public class UserPermissionManagementProvider(
    PermissionService permissionGrantRepository,
    ICurrentTenant currentTenant) : PermissionManagementProvider(permissionGrantRepository, currentTenant)
{
    public override string Name => UserPermissionValueProvider.ProviderName;
}
