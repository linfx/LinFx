using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LinFx.Extensions.DependencyInjection;

//TODO: Make DefaultConventionalRegistrar extensible, so we can only define GetLifeTimeOrNull to contribute to the convention. This can be more performant!
public class DefaultConventionalRegistrar : ConventionalRegistrarBase
{
    public override void AddType(IServiceCollection services, Type type)
    {
        // 判断类型是否标注了 DisableConventionalRegistration 特性，如果有标注，则跳过。
        if (IsConventionalRegistrationDisabled(type))
            return;

        var serviceAttribute = GetServiceAttributeOrNull(type);
        var lifeTime = GetLifeTimeOrNull(type, serviceAttribute);

        if (lifeTime == null)
            return;

        var exposedServiceTypes = GetExposedServiceTypes(type);

        TriggerServiceExposing(services, type, exposedServiceTypes);

        foreach (var exposedServiceType in exposedServiceTypes)
        {
            var serviceDescriptor = CreateServiceDescriptor(
                type,
                exposedServiceType,
                exposedServiceTypes,
                lifeTime.Value
            );

            if (serviceAttribute?.ReplaceServices == true)
            {
                // 替换服务。
                services.Replace(serviceDescriptor);
            }
            else if (serviceAttribute?.TryRegister == true)
            {
                // 注册服务。
                services.TryAdd(serviceDescriptor);
            }
            else
            {
                // 注册服务。
                services.Add(serviceDescriptor);
            }
        }
    }
}
