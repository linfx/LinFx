using LinFx.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 方法调用授权服务
/// </summary>
[Service]
public class MethodInvocationAuthorizationService : IMethodInvocationAuthorizationService
{
    private readonly IAuthorizationPolicyProvider _authorizationPolicyProvider;
    private readonly IAuthorizationService _authorizationService;

    public MethodInvocationAuthorizationService(
        IAuthorizationPolicyProvider authorizationPolicyProvider,
        IAuthorizationService authorizationService)
    {
        _authorizationPolicyProvider = authorizationPolicyProvider;
        _authorizationService = authorizationService;
    }

    public async Task CheckAsync(MethodInvocationAuthorizationContext context)
    {
        if (AllowAnonymous(context))
            return;

        var authorizationPolicy = await AuthorizationPolicy.CombineAsync(
            _authorizationPolicyProvider,
            GetAuthorizationDataAttributes(context.Method)
        );

        if (authorizationPolicy == null)
            return;

        //await _authorizationService.CheckAsync(authorizationPolicy);
    }

    protected virtual bool AllowAnonymous(MethodInvocationAuthorizationContext context)
    {
        return context.Method.GetCustomAttributes(true).OfType<IAllowAnonymous>().Any();
    }

    protected virtual IEnumerable<IAuthorizeData> GetAuthorizationDataAttributes(MethodInfo methodInfo)
    {
        var attributes = methodInfo
            .GetCustomAttributes(true)
            .OfType<IAuthorizeData>();

        if (methodInfo.IsPublic && methodInfo.DeclaringType != null)
        {
            attributes = attributes
                .Union(
                    methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<IAuthorizeData>()
                );
        }

        return attributes;
    }
}
