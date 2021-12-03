namespace LinFx.Extensions.AuditLogging
{
    public static class AuditLoggingDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Auditing_";

        public static string DbSchema { get; set; }

        public const string ConnectionStringName = "AuditLogging";
    }
}
