namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 对象访问器
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObjectAccessor<out T>
{
    T Value { get; }
}
