using LinFx.Extensions.Auditing;

namespace LinFx.Domain.Entities.Auditing;

public class AuditLogScope : IAuditLogScope
{
    public AuditLogInfo Log { get; }

    public AuditLogScope(AuditLogInfo log)
    {
        Log = log;
    }
}
