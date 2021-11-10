using LinFx;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 注册程序集下实现依赖注入接口的类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        public static LinFxBuilder AddAssembly(this LinFxBuilder builder, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(type => type != null && type.IsClass && !type.IsAbstract && !type.IsGenericType).ToArray();
            AddTypes(builder.Services, types);
            return builder;
        }

        private static void AddTypes(IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                AddType(services, type);
            }
        }

        private static void AddType(IServiceCollection services, Type type)
        {
            var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>(true);

            if (serviceAttribute == null)
                return;

            var lifeTime = serviceAttribute.Lifetime;

            var serviceTypes = ExposedServiceExplorer.GetExposedServices(type);

            foreach (var serviceType in serviceTypes)
            {
                var serviceDescriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime);

                if (serviceAttribute?.ReplaceServices == true)
                {
                    services.Replace(serviceDescriptor);
                }
                else if (serviceAttribute?.TryRegister == true)
                {
                    services.TryAdd(serviceDescriptor);
                }
                else
                {
                    services.Add(serviceDescriptor);
                }
            }
        }

        public static class ExposedServiceExplorer
        {
            private static readonly ExposeServicesAttribute DefaultExposeServicesAttribute =
                new ExposeServicesAttribute
                {
                    IncludeDefaults = true
                };

            public static List<Type> GetExposedServices(Type type)
            {
                return type
                    .GetCustomAttributes()
                    .OfType<IExposedServiceTypesProvider>()
                    .DefaultIfEmpty(DefaultExposeServicesAttribute)
                    .SelectMany(p => p.GetExposedServiceTypes(type))
                    .ToList();
            }
        }
    }

    public interface IExposedServiceTypesProvider
    {
        Type[] GetExposedServiceTypes(Type targetType);
    }

    public class ExposeServicesAttribute : Attribute, IExposedServiceTypesProvider
    {
        public Type[] ServiceTypes { get; }

        public bool? IncludeDefaults { get; set; }

        public bool? IncludeSelf { get; set; }

        public ExposeServicesAttribute(params Type[] serviceTypes)
        {
            ServiceTypes = serviceTypes ?? new Type[0];
        }

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
}
