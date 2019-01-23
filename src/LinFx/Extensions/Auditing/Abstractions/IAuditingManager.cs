using JetBrains.Annotations;

namespace LinFx.Extensions.Auditing
{
    public interface IAuditingManager
    {
        [CanBeNull]
        IAuditLogScope Current { get; }

        IAuditLogSaveHandle BeginScope();
    }
}