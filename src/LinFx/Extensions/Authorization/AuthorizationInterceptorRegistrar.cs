using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 授权拦截器注册
/// </summary>
public static class AuthorizationInterceptorRegistrar
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="context">拦截器注册上下文</param>
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<AuthorizationInterceptor>();
        }
    }

    /// <summary>
    /// 是否拦截
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool ShouldIntercept(Type type) => !DynamicProxyIgnoreTypes.Contains(type) && (type.IsDefined(typeof(AuthorizeAttribute), true) || AnyMethodHasAuthorizeAttribute(type));

    private static bool AnyMethodHasAuthorizeAttribute(Type implementationType) => implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasAuthorizeAttribute);

    private static bool HasAuthorizeAttribute(MemberInfo methodInfo) => methodInfo.IsDefined(typeof(AuthorizeAttribute), true);
}
