using EmitMapper;

namespace LinFx.Extensions.ObjectMapping
{
    public class ObjectMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>().Map(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>().Map(source, destination);
        }
    }
}
