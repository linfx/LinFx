using JetBrains.Annotations;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计范围
/// </summary>
public interface IAuditLogScope
{
    /// <summary>
    /// 日志信息
    /// </summary>
    [NotNull]
    AuditLogInfo Log { get; }
}
