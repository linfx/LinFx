﻿using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权拦截器
/// </summary>
public class AuthorizationInterceptor : Interceptor, ITransientDependency
{
    private readonly IMethodInvocationAuthorizationService _methodInvocationAuthorizationService;

    public AuthorizationInterceptor(IMethodInvocationAuthorizationService methodInvocationAuthorizationService)
    {
        _methodInvocationAuthorizationService = methodInvocationAuthorizationService;
    }

    public override async Task InterceptAsync(IMethodInvocation invocation)
    {
        // 将被调用的方法传入，验证是否允许访问。
        await AuthorizeAsync(invocation);
        await invocation.ProceedAsync();
    }

    /// <summary>
    /// 授权
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    protected virtual async Task AuthorizeAsync(IMethodInvocation invocation)
    {
        await _methodInvocationAuthorizationService.CheckAsync(new MethodInvocationAuthorizationContext(invocation.Method));
    }
}
