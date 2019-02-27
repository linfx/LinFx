using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionAuthorizationHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
        }
    }
}
