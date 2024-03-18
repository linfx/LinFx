using Castle.DynamicProxy;

namespace LinFx.Extensions.DynamicProxy;

/// <summary>
/// 方法拦截适配器
/// </summary>
public class CastleMethodInvocationAdapter(IInvocation invocation, IInvocationProceedInfo proceedInfo,
    Func<IInvocation, IInvocationProceedInfo, Task> proceed) : CastleMethodInvocationAdapterBase(invocation), IMethodInvocation
{
    protected IInvocationProceedInfo ProceedInfo { get; } = proceedInfo;

    protected Func<IInvocation, IInvocationProceedInfo, Task> Proceed { get; } = proceed;

    public override async Task ProceedAsync() => await Proceed(Invocation, ProceedInfo);
}
