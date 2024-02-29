using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using System.Reflection;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元拦截注册器
/// </summary>
public static class UnitOfWorkInterceptorRegistrar
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="context"></param>
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
            context.Interceptors.TryAdd<UnitOfWorkInterceptor>();
    }

    private static bool ShouldIntercept(Type type) => !DynamicProxyIgnoreTypes.Contains(type) && UnitOfWorkHelper.IsUnitOfWorkType(type.GetTypeInfo());
}
