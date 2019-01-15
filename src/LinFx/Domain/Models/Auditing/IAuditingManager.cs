using JetBrains.Annotations;

namespace LinFx.Domain.Models.Auditing
{
    public interface IAuditingManager
    {
        [CanBeNull]
        IAuditLogScope Current { get; }

        IAuditLogSaveHandle BeginScope();
    }
}