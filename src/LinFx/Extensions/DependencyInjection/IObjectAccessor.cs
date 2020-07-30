namespace LinFx.Extensions.DependencyInjection
{
    public interface IObjectAccessor<out T>
    {
        T Value { get; }
    }
}