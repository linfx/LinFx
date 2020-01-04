using EmitMapper;
using EmitMapper.MappingConfiguration;
using System.Collections.Generic;

namespace System
{
    public static class ObjectMapperExtensions
    {
        public static TDestination MapTo<TDestination>(this object source) 
            where TDestination : class
        {
            var config = new DefaultMapConfig();

            if(typeof(TDestination).IsAssignableFrom(typeof(IEnumerable<>)))
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
    }
}
