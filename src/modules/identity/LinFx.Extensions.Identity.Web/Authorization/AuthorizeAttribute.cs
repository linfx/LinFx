using LinFx.Extensions.Identity.Data;
using LinFx.Extensions.Identity.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Identity.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(RoleDefine.Admin))
                return;

            var ctrl = context.ActionDescriptor as ControllerActionDescriptor;
            var policyName = $"{IdentityPermissions.GroupName}.{ctrl.ControllerName}.{ctrl.ActionName}";
            var claimValues = policyName.Split(new char[] { '.' }, StringSplitOptions.None);
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new ClaimsAuthorizationRequirement(claimValues[0], new[] { policyName }));
            if (!authorizationResult.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
