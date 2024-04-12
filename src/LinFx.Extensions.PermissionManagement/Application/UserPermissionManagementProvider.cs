using LinFx.Extensions.Authorization.Permissions;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.PermissionManagement;

/// <summary>
/// 管理用户基础权限
/// </summary>
/// <param name="currentTenant"></param>
/// <param name="service"></param>
[Service(ServiceLifetime.Singleton)]
public class UserPermissionManagementProvider(PermissionService service, ICurrentTenant currentTenant) : PermissionManagementProvider(currentTenant, service)
{
    public override string Name => UserPermissionValueProvider.ProviderName;
}
