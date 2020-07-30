namespace LinFx.Extensions.Auditing
{
    public interface IAuditingManager
    {
        IAuditLogScope Current { get; }

        IAuditLogSaveHandle BeginScope();
    }
}