using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 方法调用授权服务
/// </summary>
public class MethodInvocationAuthorizationService(IAuthorizationPolicyProvider authorizationPolicyProvider, IAuthorizationService authorizationService) : IMethodInvocationAuthorizationService
{
    private readonly IAuthorizationPolicyProvider authorizationPolicyProvider = authorizationPolicyProvider;
    private readonly IAuthorizationService authorizationService = authorizationService;

    public async Task CheckAsync(MethodInvocationAuthorizationContext context)
    {
        if (AllowAnonymous(context))
            return;

        var authorizationPolicy = await AuthorizationPolicy.CombineAsync(authorizationPolicyProvider, GetAuthorizationDataAttributes(context.Method));
        if (authorizationPolicy == null)
            return;

        await authorizationService.CheckAsync(authorizationPolicy);
    }

    protected virtual bool AllowAnonymous(MethodInvocationAuthorizationContext context) => context.Method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();

    protected virtual IEnumerable<IAuthorizeData> GetAuthorizationDataAttributes(MethodInfo methodInfo)
    {
        var attributes = methodInfo.GetCustomAttributes(true).OfType<IAuthorizeData>();
        if (methodInfo.IsPublic && methodInfo.DeclaringType != null)
        {
            attributes = attributes.Union(methodInfo.DeclaringType.GetCustomAttributes(true).OfType<IAuthorizeData>());
        }
        return attributes;
    }
}
