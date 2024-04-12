namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 服务类型提供者
/// </summary>
public interface IExposedServiceTypesProvider
{
    Type[] GetExposedServiceTypes(Type targetType);
}
