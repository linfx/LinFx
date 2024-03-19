using LinFx.Collections;
using LinFx.Extensions.DynamicProxy;

namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 拦截器注册上下文
/// </summary>
public interface IOnServiceRegistredContext
{
    /// <summary>
    /// 拦截器
    /// </summary>
    ITypeList<IInterceptor> Interceptors { get; }

    /// <summary>
    /// 类型
    /// </summary>
    Type ImplementationType { get; }
}
