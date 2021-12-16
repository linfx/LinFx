using JetBrains.Annotations;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计范围
/// </summary>
public interface IAuditLogScope
{
    /// <summary>
    /// 审计信息
    /// </summary>
    [NotNull]
    AuditLogInfo Log { get; }
}
