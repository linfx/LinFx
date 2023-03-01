using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 拦截注册器
/// </summary>
public static class AuditingInterceptorRegistrar
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="context"></param>
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        // 如果类型允许被审计日志拦截器所拦截，则在类型关联的拦截器上下文当中添加审计日志拦截器。
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<AuditingInterceptor>();
        }
    }

    /// <summary>
    /// 是否拦截
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool ShouldIntercept(Type type)
    {
        if (DynamicProxyIgnoreTypes.Contains(type))
            return false;

        if (ShouldAuditTypeByDefaultOrNull(type) == true)
            return true;

        // 如果类型的任意方法启用了 Auditied 特性，则应用拦截器。
        if (type.GetMethods().Any(m => m.IsDefined(typeof(AuditedAttribute), true)))
            return true;

        return false;
    }

    //TODO: Move to a better place
    public static bool? ShouldAuditTypeByDefaultOrNull(Type type)
    {
        //TODO: In an inheritance chain, it would be better to check the attributes on the top class first.

        // 判断类型是否使用了 Audited 特性，使用了则应用审计日志拦截器。
        if (type.IsDefined(typeof(AuditedAttribute), true))
            return true;

        // 判断类型是否使用了 DisableAuditing 特性，使用了则不关联拦截器。
        if (type.IsDefined(typeof(DisableAuditingAttribute), true))
            return false;

        // 如果类型实现了 IAuditingEnabled 接口，则启用拦截器。
        if (typeof(IAuditingEnabled).IsAssignableFrom(type))
            return true;

        return null;
    }
}
