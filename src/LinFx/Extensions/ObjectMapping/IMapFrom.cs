namespace LinFx.Extensions.ObjectMapping
{
    public interface IMapFrom<in TSource>
    {
        void MapFrom(TSource source);
    }
}