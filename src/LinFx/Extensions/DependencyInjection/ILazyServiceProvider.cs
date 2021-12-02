using System;

namespace LinFx.Extensions.DependencyInjection;

public interface ILazyServiceProvider
{
    T LazyGetRequiredService<T>();

    object LazyGetRequiredService(Type serviceType);

    T LazyGetService<T>();

    object LazyGetService(Type serviceType);

    T LazyGetService<T>(T defaultValue);

    object LazyGetService(Type serviceType, object defaultValue);

    object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory);

    T LazyGetService<T>(Func<IServiceProvider, object> factory);
}
