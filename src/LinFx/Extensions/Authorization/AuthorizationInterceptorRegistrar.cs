using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Reflection;

namespace LinFx.Extensions.Authorization
{
    /// <summary>
    /// 权限验证拦截器注册
    /// </summary>
    public static class AuthorizationInterceptorRegistrar
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return !DynamicProxyIgnoreTypes.Contains(type) && (type.IsDefined(typeof(AuthorizeAttribute), true) || AnyMethodHasAuthorizeAttribute(type));
        }

        private static bool AnyMethodHasAuthorizeAttribute(Type implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasAuthorizeAttribute);
        }

        private static bool HasAuthorizeAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(AuthorizeAttribute), true);
        }
    }
}
