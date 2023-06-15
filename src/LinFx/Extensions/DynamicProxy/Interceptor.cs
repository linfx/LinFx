namespace LinFx.Extensions.DynamicProxy;

/// <summary>
/// 拦截器
/// </summary>
public abstract class Interceptor : IInterceptor
{
    /// <summary>
    /// 拦截
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    public abstract Task InterceptAsync(IMethodInvocation invocation);
}
