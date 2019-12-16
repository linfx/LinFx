using LinFx.Security.Authorization.Permissions;

namespace LinFx.Extensions.Identity.Permissions
{
    public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityGroup = context.AddGroup(IdentityPermissions.GroupName, L("Permission:IdentityManagement"));

            var rolesPermission = identityGroup.AddPermission(IdentityPermissions.Roles.Default, L("Permission:RoleManagement"));
            rolesPermission.AddChild(IdentityPermissions.Roles.Create, L("Permission:Create"));
            rolesPermission.AddChild(IdentityPermissions.Roles.Edit, L("Permission:Edit"));
            rolesPermission.AddChild(IdentityPermissions.Roles.Delete, L("Permission:Delete"));

            var usersPermission = identityGroup.AddPermission(IdentityPermissions.Users.Default, L("Permission:UserManagement"));
            usersPermission.AddChild(IdentityPermissions.Users.Create, L("Permission:Create"));
            usersPermission.AddChild(IdentityPermissions.Users.Edit, L("Permission:Edit"));
            usersPermission.AddChild(IdentityPermissions.Users.Delete, L("Permission:Delete"));
        }

        private static string L(string name)
        {
            return name;
        }
    }
}
