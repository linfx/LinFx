using System.Threading.Tasks;

namespace LinFx.Extensions.DependencyInjection
{
    public abstract class Interceptor : IInterceptor
    {
        public abstract Task InterceptAsync(IMethodInvocation invocation);
    }
}