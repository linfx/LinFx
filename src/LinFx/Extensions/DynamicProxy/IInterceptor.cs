namespace LinFx.Extensions.DynamicProxy;

/// <summary>
/// 拦截器接口
/// </summary>
public interface IInterceptor
{
    /// <summary>
    /// 异步方法拦截
    /// </summary>
    /// <param name="invocation">调用方法</param>
    /// <returns></returns>
    Task InterceptAsync(IMethodInvocation invocation);
}
