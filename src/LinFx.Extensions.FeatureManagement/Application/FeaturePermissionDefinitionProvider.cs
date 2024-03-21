using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extensions.FeatureManagement;

public class FeaturePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var featureManagementGroup = context.AddGroup(FeatureManagementPermissions.GroupName, L("Permission:FeatureManagement"));
        featureManagementGroup.AddPermission(FeatureManagementPermissions.ManageHostFeatures, L("Permission:FeatureManagement.ManageHostFeatures"));
    }
}
