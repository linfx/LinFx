namespace LinFx.Extensions.DependencyInjection;

public static class ExposedServiceExplorer
{
    private static readonly ExposeServicesAttribute DefaultExposeServicesAttribute = new()
    {
        IncludeDefaults = true,
        IncludeSelf = true
    };

    /// <summary>
    /// 获取服务类型列表
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<Type> GetExposedServices(Type type)
    {
        return type
            .GetCustomAttributes(true)
            .OfType<IExposedServiceTypesProvider>()
            .DefaultIfEmpty(DefaultExposeServicesAttribute)
            .SelectMany(p => p.GetExposedServiceTypes(type))
            .Distinct()
            .ToList();
    }
}