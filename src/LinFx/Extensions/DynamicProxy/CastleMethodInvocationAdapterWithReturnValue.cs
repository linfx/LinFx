using Castle.DynamicProxy;

namespace LinFx.Extensions.DynamicProxy;

public class CastleMethodInvocationAdapterWithReturnValue<TResult>(IInvocation invocation,
    IInvocationProceedInfo proceedInfo,
    Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed) : CastleMethodInvocationAdapterBase(invocation), IMethodInvocation
{
    protected IInvocationProceedInfo ProceedInfo { get; } = proceedInfo;

    protected Func<IInvocation, IInvocationProceedInfo, Task<TResult>> Proceed { get; } = proceed;

    public override async Task ProceedAsync() => ReturnValue = await Proceed(Invocation, ProceedInfo);
}
