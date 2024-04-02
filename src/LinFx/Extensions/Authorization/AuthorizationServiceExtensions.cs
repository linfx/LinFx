using LinFx.Security.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace LinFx.Extensions.Authorization;

public static class AuthorizationServiceExtensions
{
    public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName) => AuthorizeAsync(authorizationService, authorizationService.CurrentPrincipal, policyName);

    //public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
    //{
    //    return authorizationService.AuthorizeAsync(
    //        authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
    //        resource,
    //        requirement
    //    );
    //}

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object? resource, string policyName) => await authorizationService.AuthorizeAsync(authorizationService.CurrentPrincipal, resource, policyName);

    public static async Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy) => await AuthorizeAsync(authorizationService, null, policy);

    public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object? resource, AuthorizationPolicy policy) => authorizationService.AuthorizeAsync(authorizationService.CurrentPrincipal, resource, policy);

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy) => (await authorizationService.AuthorizeAsync(policy)).Succeeded;

    //public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, object resource, string policyName) => (await authorizationService.AuthorizeAsync(resource, policyName)).Succeeded;

    public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName) => (await authorizationService.AuthorizeAsync(policyName)).Succeeded;

    public static async Task CheckAsync(this IAuthorizationService authorizationService, AuthorizationPolicy policy)
    {
        if (!await authorizationService.IsGrantedAsync(policy))
        {
            throw new AuthorizationException(code: AuthorizationErrorCodes.GivenPolicyHasNotGranted);
        }
    }

    public static async Task CheckAsync(this IAuthorizationService authorizationService, string policyName)
    {
        if (!await authorizationService.IsGrantedAsync(policyName))
        {
            throw new AuthorizationException(code: AuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName).WithData("PolicyName", policyName);
        }
    }

    //public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName)
    //{
    //    return (await authorizationService.AuthorizeAsync(policyName)).Succeeded;
    //}
}
