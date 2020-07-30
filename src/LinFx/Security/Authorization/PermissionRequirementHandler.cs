using LinFx.Security.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization
{
    /// <summary>
    /// 权限处理器
    /// </summary>
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        // 这里通过权限检查器来确定当前用户是否拥有某个权限。
        private readonly IPermissionChecker _permissionChecker;

        public PermissionRequirementHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 如果当前用户拥有某个权限，则通过 Contxt.Succeed() 通过授权验证。
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
        }
    }
}
