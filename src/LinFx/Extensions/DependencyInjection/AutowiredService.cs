using System.Reflection;

namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 注入服务
/// </summary>
/// <param name="serviceProvider"></param>
public class AutowiredService(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public void Autowired(object service)
    {
        var serviceType = service.GetType();

        //字段赋值
        foreach (FieldInfo field in serviceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var attribute = field.GetCustomAttribute<AutowiredAttribute>();
            if (attribute != null)
                field.SetValue(service, serviceProvider.GetService(field.FieldType));
        }

        //属性赋值
        foreach (PropertyInfo property in serviceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var attribute = property.GetCustomAttribute<AutowiredAttribute>();
            if (attribute != null)
                property.SetValue(service, serviceProvider.GetService(property.PropertyType));
        }
    }
}
