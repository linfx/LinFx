namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 对象访问器
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectAccessor<T> : IObjectAccessor<T>
{
    public T Value { get; set; }

    public ObjectAccessor() { }

    public ObjectAccessor(T obj) => Value = obj;
}
