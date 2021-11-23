using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.DynamicProxy
{
    public class CastleMethodInvocationAdapter : CastleMethodInvocationAdapterBase, IMethodInvocation
    {
        protected IInvocationProceedInfo ProceedInfo { get; }
        protected Func<IInvocation, IInvocationProceedInfo, Task> Proceed { get; }

        public CastleMethodInvocationAdapter(IInvocation invocation, IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed)
            : base(invocation)
        {
            ProceedInfo = proceedInfo;
            Proceed = proceed;
        }

        public override async Task ProceedAsync()
        {
            await Proceed(Invocation, ProceedInfo);
        }
    }
}
