using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.DynamicProxy
{
    public class CastleAsyncInterceptorAdapter<TInterceptor> : AsyncInterceptorBase where TInterceptor : IInterceptor
    {
        private readonly TInterceptor _abpInterceptor;

        public CastleAsyncInterceptorAdapter(TInterceptor abpInterceptor)
        {
            _abpInterceptor = abpInterceptor;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            //await _abpInterceptor.InterceptAsync(new CastleMethodInvocationAdapter(invocation, proceedInfo, proceed));

            throw new NotImplementedException();
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            //var adapter = new CastleMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);

            //await _abpInterceptor.InterceptAsync(adapter);

            //return (TResult)adapter.ReturnValue;

            throw new NotImplementedException();
        }
    }
}