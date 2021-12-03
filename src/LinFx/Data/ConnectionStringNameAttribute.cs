using System;
using System.Reflection;
using JetBrains.Annotations;

namespace LinFx.Data;

public class ConnectionStringNameAttribute : Attribute
{
    [NotNull]
    public string Name { get; }

    public ConnectionStringNameAttribute([NotNull] string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static string GetConnStringName<T>()
    {
        return GetConnStringName(typeof(T));
    }

    public static string GetConnStringName(Type type)
    {
        var nameAttribute = type.GetTypeInfo().GetCustomAttribute<ConnectionStringNameAttribute>();

        if (nameAttribute == null)
        {
            return type.FullName;
        }

        return nameAttribute.Name;
    }
}
