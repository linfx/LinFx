namespace LinFx.Extensions.ExceptionHandling.Localization;

public class ExceptionLocalizationOptions
{
    public Dictionary<string, Type> ErrorCodeNamespaceMappings { get; } = [];

    public void MapCodeNamespace(string errorCodeNamespace, Type type) => ErrorCodeNamespaceMappings[errorCodeNamespace] = type;
}
