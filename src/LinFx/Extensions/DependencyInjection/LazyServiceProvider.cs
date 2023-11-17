using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.DependencyInjection;

public class LazyServiceProvider : ILazyServiceProvider, ITransientDependency
{
    protected IDictionary<Type, object> CachedServices { get; set; }

    protected IServiceProvider ServiceProvider { get; set; }

    public LazyServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CachedServices = new Dictionary<Type, object>();
    }

    public virtual T LazyGetRequiredService<T>() => (T)LazyGetRequiredService(typeof(T));

    public virtual object LazyGetRequiredService(Type serviceType) => CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));

    public virtual T LazyGetService<T>() => (T)LazyGetService(typeof(T));

    public virtual object LazyGetService(Type serviceType) => CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetService(serviceType));

    public virtual T LazyGetService<T>(T defaultValue) => (T)LazyGetService(typeof(T), defaultValue);

    public virtual object LazyGetService(Type serviceType, object defaultValue) => LazyGetService(serviceType) ?? defaultValue;

    public virtual T LazyGetService<T>(Func<IServiceProvider, object> factory) => (T)LazyGetService(typeof(T), factory);

    public virtual object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory) => CachedServices.GetOrAdd(serviceType, () => factory(ServiceProvider));
}
