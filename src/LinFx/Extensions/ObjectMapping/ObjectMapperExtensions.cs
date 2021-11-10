using EmitMapper;
using EmitMapper.MappingConfiguration;
using LinFx.Extensions.ObjectMapping;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
    public static class ObjectMapperExtensions
    {
        public static TDestination MapTo<TDestination>(this object source)
            where TDestination : class
        {
            var config = new DefaultMapConfig();

            if (typeof(TDestination).IsAssignableFrom(typeof(IEnumerable<>)))
                config = config.ConvertGeneric(typeof(IEnumerable<>), typeof(IEnumerable<>), new DefaultCustomConverterProvider(typeof(TDestination)));

            var item = ObjectMapperManager.DefaultInstance.GetMapperImpl(source.GetType(), typeof(TDestination), config).Map(source);
            return item.As<TDestination>();
        }

        public static object MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>().Map(source, destination);
        }

        private static readonly MethodInfo MapToNewObjectMethod;
        private static readonly MethodInfo MapToExistingObjectMethod;

        static ObjectMapperExtensions()
        {
            var methods = typeof(IObjectMapper).GetMethods();
            foreach (var method in methods)
            {
                if (method.Name == nameof(IObjectMapper.Map) && method.IsGenericMethodDefinition)
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 1)
                    {
                        MapToNewObjectMethod = method;
                    }
                    else if (parameters.Length == 2)
                    {
                        MapToExistingObjectMethod = method;
                    }
                }
            }
        }

        public static object Map(this IObjectMapper objectMapper, Type sourceType, Type destinationType, object source)
        {
            return MapToNewObjectMethod.MakeGenericMethod(sourceType, destinationType).Invoke(objectMapper, new[] { source });
        }

        public static object Map(this IObjectMapper objectMapper, Type sourceType, Type destinationType, object source, object destination)
        {
            return MapToExistingObjectMethod.MakeGenericMethod(sourceType, destinationType).Invoke(objectMapper, new[] { source, destination });
        }
    }
}
