using LinFx.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionRegistrationActionExtensions
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="services"></param>
    /// <param name="registrationAction"></param>
    public static void OnRegistred(this IServiceCollection services, Action<IOnServiceRegistredContext> registrationAction) => GetOrCreateRegistrationActionList(services).Add(registrationAction);

    public static ServiceRegistrationActionList GetRegistrationActionList(this IServiceCollection services) => GetOrCreateRegistrationActionList(services);

    private static ServiceRegistrationActionList GetOrCreateRegistrationActionList(IServiceCollection services)
    {
        var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceRegistrationActionList>>()?.Value;
        if (actionList == null)
        {
            actionList = new ServiceRegistrationActionList();
            services.AddObjectAccessor(actionList);
        }
        return actionList;
    }

    // OnExposing
    public static void OnExposing(this IServiceCollection services, Action<IOnServiceExposingContext> exposeAction) => GetOrCreateExposingList(services).Add(exposeAction);

    public static ServiceExposingActionList GetExposingActionList(this IServiceCollection services) => GetOrCreateExposingList(services);

    private static ServiceExposingActionList GetOrCreateExposingList(IServiceCollection services)
    {
        var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceExposingActionList>>()?.Value;
        if (actionList == null)
        {
            actionList = new ServiceExposingActionList();
            services.AddObjectAccessor(actionList);
        }

        return actionList;
    }
}
