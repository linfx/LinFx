using System.Threading.Tasks;

namespace LinFx.Extensions.DynamicProxy
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public abstract class Interceptor : IInterceptor
    {
        public abstract Task InterceptAsync(IMethodInvocation invocation);
    }
}