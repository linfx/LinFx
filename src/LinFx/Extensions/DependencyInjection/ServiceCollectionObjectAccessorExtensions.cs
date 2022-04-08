using LinFx.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionObjectAccessorExtensions
{
    /// <summary>
    /// 注册对象访问器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ObjectAccessor<T> TryAddObjectAccessor<T>(this IServiceCollection services)
    {
        if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
            return services.GetSingletonInstance<ObjectAccessor<T>>();

        return services.AddObjectAccessor<T>();
    }

    /// <summary>
    /// 注册对象访问器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>());
    }

    /// <summary>
    /// 注册对象访问器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj)
    {
        return services.AddObjectAccessor(new ObjectAccessor<T>(obj));
    }

    /// <summary>
    /// 注册对象访问器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="accessor"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
    {
        // 判断某个特定泛型的对象访问器是否被注册，如果被注册直接抛出异常，没有则继续。
        if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
            throw new Exception("An object accessor is registered before for type: " + typeof(T).AssemblyQualifiedName);

        // Add to the beginning for fast retrieve
        // 将某个特定类型的对象访问器作为单例注册到 IoC 容器的头部，方便快速检索。
        services.Insert(0, ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));
        services.Insert(0, ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));

        return accessor;
    }

    public static T GetObjectOrNull<T>(this IServiceCollection services)
        where T : class
    {
        return services.GetSingletonInstanceOrNull<IObjectAccessor<T>>()?.Value;
    }

    public static T GetObject<T>(this IServiceCollection services)
        where T : class
    {
        return services.GetObjectOrNull<T>() ?? throw new Exception($"Could not find an object of {typeof(T).AssemblyQualifiedName} in services. Be sure that you have used AddObjectAccessor before!");
    }
}
