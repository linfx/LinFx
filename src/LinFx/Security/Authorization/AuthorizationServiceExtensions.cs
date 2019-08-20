using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        //public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, string policyName)
        //{
        //    return AuthorizeAsync(
        //        authorizationService,
        //        authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
        //        policyName
        //    );
        //}

        //public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, IAuthorizationRequirement requirement)
        //{
        //    return authorizationService.AuthorizeAsync(
        //        authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
        //        resource,
        //        requirement
        //    );
        //}

        //public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService authorizationService, object resource, AuthorizationPolicy policy)
        //{
        //    return authorizationService.AuthorizeAsync(
        //        authorizationService.AsAbpAuthorizationService().CurrentPrincipal,
        //        resource,
        //        policy
        //    );
        //}

        //public static async Task<bool> IsGrantedAsync(this IAuthorizationService authorizationService, string policyName)
        //{
        //    return (await authorizationService.AuthorizeAsync(policyName)).Succeeded;
        //}
    }
}
