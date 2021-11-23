using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using System;
using System.Reflection;

namespace LinFx.Extensions.Uow
{
    /// <summary>
    /// 拦截器注册
    /// </summary>
    public static class UnitOfWorkInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<UnitOfWorkInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return !DynamicProxyIgnoreTypes.Contains(type) && UnitOfWorkHelper.IsUnitOfWorkType(type.GetTypeInfo());
        }
    }
}