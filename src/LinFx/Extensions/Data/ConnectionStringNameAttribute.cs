using JetBrains.Annotations;
using System.Reflection;

namespace LinFx.Extensions.Data;

public class ConnectionStringNameAttribute : Attribute
{
    [NotNull]
    public string Name { get; }

    public ConnectionStringNameAttribute([NotNull] string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

    public static string GetConnStringName<T>() => GetConnStringName(typeof(T));

    /// <summary>
    /// 获取链接字符串
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetConnStringName(Type type)
    {
        var nameAttribute = type.GetTypeInfo().GetCustomAttribute<ConnectionStringNameAttribute>();
        if (nameAttribute == null)
            return type.FullName;

        return nameAttribute.Name;
    }
}
