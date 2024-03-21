using LinFx.Extensions.Data;

namespace LinFx.Extensions.FeatureManagement;

public static class FeatureManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = CommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = CommonDbProperties.DbSchema;

    public const string ConnectionStringName = "FeatureManagement";
}
