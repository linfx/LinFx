using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志管理器
/// </summary>
public interface IAuditingManager
{
    /// <summary>
    /// 当前审计范围
    /// </summary>
    [AllowNull]
    IAuditLogScope Current { get; }

    /// <summary>
    /// 开始审计
    /// </summary>
    /// <returns></returns>
    IAuditLogSaveHandle BeginScope();
}
