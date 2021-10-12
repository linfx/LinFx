using LinFx.Extensions.Authorization.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LinFx.Extensions.TenantManagement
{
    [Service(ServiceLifetime.Singleton)]
    public class TenantManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public TenantManagementPermissionDefinitionProvider(
            IStringLocalizer<TenantManagementPermissionDefinitionProvider> localizer)
            : base(localizer)
        {
        }

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
}
