using System.Threading.Tasks;

namespace LinFx.Extensions.DependencyInjection
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        Task InterceptAsync(IMethodInvocation invocation);
    }
}