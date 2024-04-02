using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.EntityFrameworkCore.Modeling;

public class ModelBuilderConfigurationOptions
{
    public string TablePrefix { get; set; }

    [AllowNull]
    public string Schema { get; set; }

    public ModelBuilderConfigurationOptions(
        string tablePrefix = default,
        [AllowNull] string schema = default)
    {
        TablePrefix = tablePrefix;
        Schema = schema;
    }
}
