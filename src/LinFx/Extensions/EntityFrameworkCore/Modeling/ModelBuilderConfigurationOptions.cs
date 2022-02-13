using JetBrains.Annotations;

namespace LinFx.Extensions.EntityFrameworkCore.Modeling;

public class ModelBuilderConfigurationOptions
{
    [NotNull]
    public string TablePrefix { get; set; }

    [CanBeNull]
    public string Schema { get; set; }

    public ModelBuilderConfigurationOptions(
        [NotNull] string tablePrefix = default,
        [CanBeNull] string schema = default)
    {
        Check.NotNull(tablePrefix, nameof(tablePrefix));

        TablePrefix = tablePrefix;
        Schema = schema;
    }
}
