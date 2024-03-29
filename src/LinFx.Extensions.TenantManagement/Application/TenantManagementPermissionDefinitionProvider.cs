using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 定义租户管理权限
/// </summary>
public class TenantManagementPermissionDefinitionProvider(IServiceProvider serviceProvider) : PermissionDefinitionProvider(serviceProvider)
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var tenantManagementGroup = context.AddGroup(TenantManagementPermissions.GroupName, L("Permission:TenantManagement"));
        var tenantsPermission = tenantManagementGroup.AddPermission(TenantManagementPermissions.Tenants.Default, L("Permission:TenantManagement"));
        tenantsPermission.AddChild(TenantManagementPermissions.Tenants.Create, L("Permission:Create"));
        tenantsPermission.AddChild(TenantManagementPermissions.Tenants.Update, L("Permission:Edit"));
        tenantsPermission.AddChild(TenantManagementPermissions.Tenants.Delete, L("Permission:Delete"));
        //tenantsPermission.AddChild(TenantManagementPermissions.Tenants.ManageFeatures, L("Permission:ManageFeatures"));
        //tenantsPermission.AddChild(TenantManagementPermissions.Tenants.ManageConnectionStrings, L("Permission:ManageConnectionStrings"));
    }
}
