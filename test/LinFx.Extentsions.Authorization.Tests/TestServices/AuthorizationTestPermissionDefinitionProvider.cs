using LinFx.Extensions.Authorization.Permissions;

namespace LinFx.Extentsions.Authorization.Tests.TestServices
{
    public class AuthorizationTestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var getGroup = context.GetGroupOrNull("TestGetGroup");
            if (getGroup == null)
            {
                getGroup = context.AddGroup("TestGetGroup");
            }

            var group = context.AddGroup("TestGroup");
            group.AddPermission("MyAuthorizedService1");

            //group.GetPermissionOrNull("MyAuthorizedService1").ShouldNotBeNull();

            //context.RemoveGroup("TestGetGroup");
        }
    }
}
