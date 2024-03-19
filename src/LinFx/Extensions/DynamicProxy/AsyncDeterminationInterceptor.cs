using Castle.DynamicProxy;

namespace LinFx.Extensions.DynamicProxy;

/// <summary>
/// 异步拦截器
/// </summary>
/// <typeparam name="TInterceptor"></typeparam>
public class AsyncDeterminationInterceptor<TInterceptor>(TInterceptor anterceptor) : AsyncDeterminationInterceptor(new CastleAsyncInterceptorAdapter<TInterceptor>(anterceptor)) 
    where TInterceptor : IInterceptor
{
}
