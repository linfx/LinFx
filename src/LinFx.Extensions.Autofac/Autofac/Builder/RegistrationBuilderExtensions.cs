using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.DynamicProxy;
using LinFx.Extensions.Modularity;

namespace Autofac.Builder;

public static class RegistrationBuilderExtensions
{
    public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ConfigureConventions<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
            IModuleContainer moduleContainer,
            ServiceRegistrationActionList registrationActionList)
        where TActivatorData : ReflectionActivatorData
    {
        var serviceType = registrationBuilder.RegistrationData.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;
        if (serviceType == null)
            return registrationBuilder;

        var implementationType = registrationBuilder.ActivatorData.ImplementationType;
        if (implementationType == null)
            return registrationBuilder;

        registrationBuilder = registrationBuilder.EnablePropertyInjection(moduleContainer, implementationType);
        registrationBuilder = registrationBuilder.InvokeRegistrationActions(registrationActionList, serviceType, implementationType);

        return registrationBuilder;
    }

    /// <summary>
    /// 调用传入 Action
    /// </summary>
    /// <typeparam name="TLimit"></typeparam>
    /// <typeparam name="TActivatorData"></typeparam>
    /// <typeparam name="TRegistrationStyle"></typeparam>
    /// <param name="registrationBuilder"></param>
    /// <param name="registrationActionList"></param>
    /// <param name="serviceType"></param>
    /// <param name="implementationType"></param>
    /// <returns></returns>
    private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InvokeRegistrationActions<TLimit, TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
        ServiceRegistrationActionList registrationActionList,
        Type serviceType,
        Type implementationType)
        where TActivatorData : ReflectionActivatorData
    {
        // 构造上下文，以便去调用之前传入的 Action。
        var serviceRegistredArgs = new OnServiceRegistredContext(serviceType, implementationType);

        // 以审计日志拦截器为例，这里会调用在预加载方法传入的 AuditingInterceptorRegistrar.RegisterIfNeeded 方法。
        foreach (var registrationAction in registrationActionList)
        {
            registrationAction.Invoke(serviceRegistredArgs);
        }

        // 这里的 Interceptors 实际上就是 AuditingInterceptorRegistrar.RegisterIfNeeded 内部添加的拦截器。
        if (serviceRegistredArgs.Interceptors.Any())
        {
            registrationBuilder = registrationBuilder.AddInterceptors(registrationActionList, serviceType, serviceRegistredArgs.Interceptors);
        }

        return registrationBuilder;
    }

    /// <summary>
    /// 属性注入
    /// </summary>
    /// <typeparam name="TLimit"></typeparam>
    /// <typeparam name="TActivatorData"></typeparam>
    /// <typeparam name="TRegistrationStyle"></typeparam>
    /// <param name="registrationBuilder"></param>
    /// <param name="moduleContainer"></param>
    /// <param name="implementationType"></param>
    /// <returns></returns>
    private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> EnablePropertyInjection<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
            IModuleContainer moduleContainer,
            Type implementationType)
        where TActivatorData : ReflectionActivatorData
    {
        //Enable Property Injection only for types in an assembly containing an AbpModule
        if (moduleContainer.Modules.Any(m => m.Assembly == implementationType.Assembly))
            registrationBuilder = registrationBuilder.PropertiesAutowired(new AutowiredPropertySelector());

        return registrationBuilder;
    }

    /// <summary>
    /// 增加拦截器
    /// </summary>
    /// <typeparam name="TLimit"></typeparam>
    /// <typeparam name="TActivatorData"></typeparam>
    /// <typeparam name="TRegistrationStyle"></typeparam>
    /// <param name="registrationBuilder"></param>
    /// <param name="serviceRegistrationActionList"></param>
    /// <param name="serviceType"></param>
    /// <param name="interceptors"></param>
    /// <returns></returns>
    private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> AddInterceptors<TLimit, TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder,
            ServiceRegistrationActionList serviceRegistrationActionList,
            Type serviceType,
            IEnumerable<Type> interceptors)
        where TActivatorData : ReflectionActivatorData
    {
        if (serviceType.IsInterface)
        {
            registrationBuilder = registrationBuilder.EnableInterfaceInterceptors();
        }
        else
        {
            if (serviceRegistrationActionList.IsClassInterceptorsDisabled)
                return registrationBuilder;

            (registrationBuilder as IRegistrationBuilder<TLimit, ConcreteReflectionActivatorData, TRegistrationStyle>)?.EnableClassInterceptors();
        }

        foreach (var interceptor in interceptors)
        {
            registrationBuilder.InterceptedBy(typeof(AsyncDeterminationInterceptor<>).MakeGenericType(interceptor));
        }

        return registrationBuilder;
    }
}
