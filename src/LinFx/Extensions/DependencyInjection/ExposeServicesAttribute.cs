using System.Reflection;

namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 控制相关类服务提供
/// </summary>
public class ExposeServicesAttribute(params Type[] serviceTypes) : Attribute, IExposedServiceTypesProvider
{
    public Type[] ServiceTypes { get; } = serviceTypes ?? new Type[0];

    public bool? IncludeDefaults { get; set; }

    public bool? IncludeSelf { get; set; }

    public Type[] GetExposedServiceTypes(Type targetType)
    {
        var serviceList = ServiceTypes.ToList();

        if (IncludeDefaults == true)
        {
            foreach (var type in GetDefaultServices(targetType))
            {
                serviceList.AddIfNotContains(type);
            }

            if (IncludeSelf != false)
            {
                serviceList.AddIfNotContains(targetType);
            }
        }
        else if (IncludeSelf == true)
        {
            serviceList.AddIfNotContains(targetType);
        }

        return serviceList.ToArray();
    }

    private static List<Type> GetDefaultServices(Type type)
    {
        var serviceTypes = new List<Type>();

        foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
        {
            var interfaceName = interfaceType.Name;

            if (interfaceName.StartsWith("I"))
            {
                interfaceName = interfaceName.Right(interfaceName.Length - 1);
            }

            if (type.Name.EndsWith(interfaceName))
            {
                serviceTypes.Add(interfaceType);
            }
        }

        return serviceTypes;
    }
}
