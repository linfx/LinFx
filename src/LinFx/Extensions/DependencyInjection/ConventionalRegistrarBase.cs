using JetBrains.Annotations;
using LinFx.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinFx.Extensions.DependencyInjection;

public abstract class ConventionalRegistrarBase : IConventionalRegistrar
{
    public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
    {
        // 获得程序集内的所有类型，过滤掉抽象类和泛型类型。
        var types = AssemblyHelper
            .GetAllTypes(assembly)
            .Where(
                type => type != null &&
                        type.IsClass &&
                        !type.IsAbstract &&
                        !type.IsGenericType
            ).ToArray();

        AddTypes(services, types);
    }

    public virtual void AddTypes(IServiceCollection services, params Type[] types)
    {
        foreach (var type in types)
        {
            AddType(services, type);
        }
    }

    public abstract void AddType(IServiceCollection services, Type type);

    protected virtual bool IsConventionalRegistrationDisabled(Type type)
    {
        return type.IsDefined(typeof(DisableConventionalRegistrationAttribute), true);
    }

    protected virtual void TriggerServiceExposing(IServiceCollection services, Type implementationType, List<Type> serviceTypes)
    {
        var exposeActions = services.GetExposingActionList();
        if (exposeActions.Any())
        {
            var args = new OnServiceExposingContext(implementationType, serviceTypes);
            foreach (var action in exposeActions)
            {
                action(args);
            }
        }
    }

    /// <summary>
    /// 获取Service特性注入的类
    /// </summary>
    /// <returns></returns>
    protected virtual ServiceAttribute GetServiceAttributeOrNull(Type type)
    {
        return type.GetCustomAttribute<ServiceAttribute>(true);
    }

    /// <summary>
    /// 获取生命周期
    /// </summary>
    /// <returns></returns>
    protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] ServiceAttribute serviceAttribute)
    {
        return serviceAttribute?.Lifetime ?? GetServiceLifetimeFromClassHierarchy(type) ?? GetDefaultLifeTimeOrNull(type);
    }

    protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
    {
        if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Transient;
        }

        if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Singleton;
        }

        if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
        {
            return ServiceLifetime.Scoped;
        }

        return null;
    }

    protected virtual ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
    {
        return null;
    }

    /// <summary>
    /// 获取服务类型列表
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected virtual List<Type> GetExposedServiceTypes(Type type)
    {
        return ExposedServiceExplorer.GetExposedServices(type);
    }

    protected virtual ServiceDescriptor CreateServiceDescriptor(
        Type implementationType,
        Type exposingServiceType,
        List<Type> allExposingServiceTypes,
        ServiceLifetime lifeTime)
    {
        if (lifeTime.IsIn(ServiceLifetime.Singleton, ServiceLifetime.Scoped))
        {
            var redirectedType = GetRedirectedTypeOrNull(
                implementationType,
                exposingServiceType,
                allExposingServiceTypes
            );

            if (redirectedType != null)
            {
                return ServiceDescriptor.Describe(
                    exposingServiceType,
                    provider => provider.GetService(redirectedType),
                    lifeTime
                );
            }
        }

        return ServiceDescriptor.Describe(
            exposingServiceType,
            implementationType,
            lifeTime
        );
    }

    protected virtual Type GetRedirectedTypeOrNull(Type implementationType, Type exposingServiceType, List<Type> allExposingServiceTypes)
    {
        if (allExposingServiceTypes.Count < 2)
            return null;

        if (exposingServiceType == implementationType)
            return null;

        if (allExposingServiceTypes.Contains(implementationType))
            return implementationType;

        return allExposingServiceTypes.FirstOrDefault(t => t != exposingServiceType && exposingServiceType.IsAssignableFrom(t));
    }

}
