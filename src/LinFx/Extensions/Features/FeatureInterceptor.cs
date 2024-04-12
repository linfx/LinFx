using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征拦截器
/// </summary>
/// <param name="serviceScopeFactory"></param>
[Service]
public class FeatureInterceptor(IServiceScopeFactory serviceScopeFactory) : Interceptor
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    /// <summary>
    /// 方法拦截
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    public override async Task InterceptAsync(IMethodInvocation invocation)
    {
        await CheckFeaturesAsync(invocation);
        await invocation.ProceedAsync();
    }

    /// <summary>
    /// 特征校验
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    protected virtual async Task CheckFeaturesAsync(IMethodInvocation invocation)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IMethodInvocationFeatureCheckerService>().CheckAsync(new MethodInvocationFeatureCheckerContext(invocation.Method));
        }
    }
}
