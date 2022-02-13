using LinFx.Extensions.Data;

namespace LinFx.Extensions.TenantManagement.Domain;

public static class TenantManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = CommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = CommonDbProperties.DbSchema;

    public const string ConnectionStringName = "Default";
}
