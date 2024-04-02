using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权拦截器
/// </summary>
[Service]
public class AuthorizationInterceptor(IMethodInvocationAuthorizationService methodInvocationAuthorizationService) : Interceptor
{
    private readonly IMethodInvocationAuthorizationService _methodInvocationAuthorizationService = methodInvocationAuthorizationService;

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
    protected virtual Task AuthorizeAsync(IMethodInvocation invocation) => _methodInvocationAuthorizationService.CheckAsync(new MethodInvocationAuthorizationContext(invocation.Method));
}
