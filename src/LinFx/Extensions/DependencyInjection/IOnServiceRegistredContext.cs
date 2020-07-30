using LinFx.Collections;
using System;

namespace LinFx.Extensions.DependencyInjection
{
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
}