using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 定义租户管理权限
/// </summary>
public class TenantManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(TenantManagementPermissions.GroupName, L["Permission:TenantManagement"]);
        var permission = group.AddPermission(TenantManagementPermissions.Tenants.Default, L["Permission:TenantManagement"]);
        permission.AddChild(TenantManagementPermissions.Tenants.Create, L["Permission:Create"]);
        permission.AddChild(TenantManagementPermissions.Tenants.Update, L["Permission:Edit"]);
        permission.AddChild(TenantManagementPermissions.Tenants.Delete, L["Permission:Delete"]);
    }
}
