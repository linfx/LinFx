﻿using System.Reflection;

namespace LinFx.Utils;

public static class ProxyHelper
{
    private const string ProxyNamespace = "Castle.Proxies";

    /// <summary>
    /// Returns dynamic proxy target object if this is a proxied object, otherwise returns the given object. 
    /// It supports Castle Dynamic Proxies.
    /// </summary>
    public static object UnProxy(object obj)
    {
        if (obj.GetType().Namespace != ProxyNamespace)
            return obj;

        var targetField = obj.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .FirstOrDefault(f => f.Name == "__target");

        if (targetField == null)
            return obj;

        return targetField.GetValue(obj);
    }

    public static Type GetUnProxiedType(object obj) => UnProxy(obj).GetType();
}
