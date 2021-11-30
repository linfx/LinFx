using System.Threading.Tasks;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志储存
/// </summary>
public interface IAuditingStore
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="auditInfo">审计日志</param>
    /// <returns></returns>
    Task SaveAsync(AuditLogInfo auditInfo);
}