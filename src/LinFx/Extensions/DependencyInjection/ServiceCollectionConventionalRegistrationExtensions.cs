﻿using LinFx.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionConventionalRegistrationExtensions
{
    public static IServiceCollection AddConventionalRegistrar(this IServiceCollection services, IConventionalRegistrar registrar)
    {
        GetOrCreateRegistrarList(services).Add(registrar);
        return services;
    }

    public static List<IConventionalRegistrar> GetConventionalRegistrars(this IServiceCollection services) => GetOrCreateRegistrarList(services);

    private static ConventionalRegistrarList GetOrCreateRegistrarList(IServiceCollection services)
    {
        var conventionalRegistrars = services.GetSingletonInstanceOrNull<IObjectAccessor<ConventionalRegistrarList>>()?.Value;
        if (conventionalRegistrars == null)
        {
            conventionalRegistrars = new ConventionalRegistrarList { new DefaultConventionalRegistrar() };
            services.AddObjectAccessor(conventionalRegistrars);
        }

        return conventionalRegistrars;
    }

    public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services) => services.AddAssembly(typeof(T).GetTypeInfo().Assembly);

    public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddAssembly(services, assembly);
        }

        return services;
    }

    public static IServiceCollection AddTypes(this IServiceCollection services, params Type[] types)
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddTypes(services, types);
        }

        return services;
    }

    public static IServiceCollection AddType<TType>(this IServiceCollection services) => services.AddType(typeof(TType));

    public static IServiceCollection AddType(this IServiceCollection services, Type type)
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddType(services, type);
        }

        return services;
    }
}
