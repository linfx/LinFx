using LinFx.Extensions.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 权限策略处理器
/// </summary>
public class PermissionRequirementHandler(IPermissionChecker permissionChecker) : AuthorizationHandler<PermissionRequirement>
{
    // 这里通过权限检查器来确定当前用户是否拥有某个权限。
    private readonly IPermissionChecker _permissionChecker = permissionChecker;

    /// <summary>
    /// 处理授权逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // 如果当前用户拥有某个权限，则通过 Contxt.Succeed() 通过授权验证。
        if (await PermissionCheckerExtensions.IsGrantedAsync(_permissionChecker, context.User, requirement.PermissionName))
            context.Succeed(requirement);
    }
}
