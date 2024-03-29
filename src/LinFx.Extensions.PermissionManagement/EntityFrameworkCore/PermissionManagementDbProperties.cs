using LinFx.Extensions.Data;

namespace LinFx.Extensions.PermissionManagement;

public static class PermissionManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = CommonDbProperties.DbTablePrefix;

    public static string? DbSchema { get; set; } = CommonDbProperties.DbSchema;

    public const string ConnectionStringName = "Default";
}
