using LinFx.Extensions.Data;

namespace LinFx.Extensions.AuditLogging.Domain;

public static class AuditLoggingDbProperties
{
    public static string DbTablePrefix { get; set; } = CommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = CommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AuditLogging";
}
