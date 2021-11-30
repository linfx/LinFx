using JetBrains.Annotations;

namespace LinFx.Extensions.Auditing;

public interface IAuditLogScope
{
    [NotNull]
    AuditLogInfo Log { get; }
}
