using LinFx.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace LinFx.Security.Authorization
{
    [Service]
    public class AuthorizationInterceptor : Interceptor
    {
        private readonly IMethodInvocationAuthorizationService _methodInvocationAuthorizationService;

        public AuthorizationInterceptor(IMethodInvocationAuthorizationService methodInvocationAuthorizationService)
        {
            _methodInvocationAuthorizationService = methodInvocationAuthorizationService;
        }

        public override async Task InterceptAsync(IMethodInvocation invocation)
        {
            await AuthorizeAsync(invocation);
            await invocation.ProceedAsync();
        }

        protected virtual async Task AuthorizeAsync(IMethodInvocation invocation)
        {
            await _methodInvocationAuthorizationService.CheckAsync(
                new MethodInvocationAuthorizationContext(
                    invocation.Method
                )
            );
        }
    }
}