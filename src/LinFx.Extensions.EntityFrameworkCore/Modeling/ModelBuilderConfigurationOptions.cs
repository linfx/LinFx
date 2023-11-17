using JetBrains.Annotations;

namespace LinFx.Extensions.EntityFrameworkCore.Modeling;

public class ModelBuilderConfigurationOptions
{
    public string TablePrefix { get; set; }

    [CanBeNull]
    public string Schema { get; set; }

    public ModelBuilderConfigurationOptions(
        string tablePrefix = default,
        [CanBeNull] string schema = default)
    {
        TablePrefix = tablePrefix;
        Schema = schema;
    }
}
