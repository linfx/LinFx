using System.Threading.Tasks;

namespace LinFx.Extensions.DependencyInjection
{
    public interface IInterceptor
    {
        Task InterceptAsync(IMethodInvocation invocation);
    }
}