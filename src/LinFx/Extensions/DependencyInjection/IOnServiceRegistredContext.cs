using LinFx.Collections;
using System;

namespace LinFx.Extensions.DependencyInjection
{
    public interface IOnServiceRegistredContext
    {
        ITypeList<IInterceptor> Interceptors { get; }

        Type ImplementationType { get; }
    }
}