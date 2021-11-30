using JetBrains.Annotations;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志管理器
/// </summary>
public interface IAuditingManager
{
    [CanBeNull]
    IAuditLogScope Current { get; }

    IAuditLogSaveHandle BeginScope();
}
