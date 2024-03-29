using LinFx.Extensions.Auditing;

namespace LinFx.Domain.Entities.Auditing;

public class AuditLogScope(AuditLogInfo log) : IAuditLogScope
{
    public AuditLogInfo Log { get; } = log;
}
