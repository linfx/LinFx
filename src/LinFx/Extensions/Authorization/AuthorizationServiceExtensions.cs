using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace LinFx.Extensions.Authorization;

public static class AuthorizationServiceExtensions
{
    public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName) => AuthorizeAsync(authorizationService, authorizationService.CurrentPrincipal, policyName);

    public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement) => authorizationService.AuthorizeAsync(authorizationService.CurrentPrincipal, resource, requirement);

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object? resource, string policyName) => await authorizationService.AuthorizeAsync(authorizationService.CurrentPrincipal, resource, policyName);

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy) => await AuthorizeAsync(authorizationService, null, policy);

    public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object? resource, AuthorizationPolicy policy) => authorizationService.AuthorizeAsync(authorizationService.CurrentPrincipal, resource, policy);

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy) => (await authorizationService.AuthorizeAsync(policy)).Succeeded;

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, string policyName) => (await authorizationService.AuthorizeAsync(resource, policyName)).Succeeded;

    /// <summary>
    /// 是否授权
    /// </summary>
    /// <param name="authorizationService">授权服务</param>
    /// <param name="policyName">策略名称</param>
    /// <returns></returns>
    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName) => (await authorizationService.AuthorizeAsync(policyName)).Succeeded;

    /// <summary>
    /// 权限校验
    /// </summary>
    /// <param name="authorizationService"></param>
    /// <param name="policy"></param>
    /// <returns></returns>
    /// <exception cref="AuthorizationException"></exception>
    public static async Task CheckAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
    {
        if (!await authorizationService.IsGrantedAsync(policy))
            throw new AuthorizationException(code: AuthorizationErrorCodes.GivenPolicyHasNotGranted);
    }

    /// <summary>
    /// 权限校验
    /// </summary>
    /// <param name="authorizationService"></param>
    /// <param name="policyName"></param>
    /// <returns></returns>
    public static async Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
    {
        if (!await authorizationService.IsGrantedAsync(policyName))
            throw new AuthorizationException(code: AuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName).WithData("PolicyName", policyName);
    }
}
