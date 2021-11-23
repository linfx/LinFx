using Castle.DynamicProxy;

namespace LinFx.Extensions.DynamicProxy
{
    public class AsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor where TInterceptor : IInterceptor
    {
        public AsyncDeterminationInterceptor(TInterceptor abpInterceptor)
            : base(new CastleAsyncInterceptorAdapter<TInterceptor>(abpInterceptor))
        {
        }
    }
}