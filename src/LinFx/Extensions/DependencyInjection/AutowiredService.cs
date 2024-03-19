using System.Reflection;

namespace LinFx.Extensions.DependencyInjection;

public class AutowiredService(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void Autowired(object service)
    {
        var serviceType = service.GetType();

        //字段赋值
        foreach (FieldInfo field in serviceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var autowiredAttr = field.GetCustomAttribute<AutowiredAttribute>();
            if (autowiredAttr != null)
            {
                field.SetValue(service, _serviceProvider.GetService(field.FieldType));
            }
        }

        //属性赋值
        foreach (PropertyInfo property in serviceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var autowiredAttr = property.GetCustomAttribute<AutowiredAttribute>();
            if (autowiredAttr != null)
            {
                property.SetValue(service, _serviceProvider.GetService(property.PropertyType));
            }
        }
    }
}
