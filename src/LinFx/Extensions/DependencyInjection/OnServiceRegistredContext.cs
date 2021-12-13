using JetBrains.Annotations;
using LinFx.Collections;
using LinFx.Extensions.DynamicProxy;
using System;

namespace LinFx.Extensions.DependencyInjection;

public class OnServiceRegistredContext : IOnServiceRegistredContext
{
    public virtual ITypeList<IInterceptor> Interceptors { get; }

    public virtual Type ServiceType { get; }

    public virtual Type ImplementationType { get; }

    public OnServiceRegistredContext(Type serviceType, [NotNull] Type implementationType)
    {
        ServiceType = Check.NotNull(serviceType, nameof(serviceType));
        ImplementationType = Check.NotNull(implementationType, nameof(implementationType));

        Interceptors = new TypeList<IInterceptor>();
    }
}
