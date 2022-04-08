using Castle.DynamicProxy;

namespace LinFx.Extensions.DynamicProxy;

/// <summary>
/// 异步拦截器
/// </summary>
/// <typeparam name="TInterceptor"></typeparam>
public class AsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor 
    where TInterceptor : IInterceptor
{
    public AsyncDeterminationInterceptor(TInterceptor anterceptor)
        : base(new CastleAsyncInterceptorAdapter<TInterceptor>(anterceptor)) { }
}
