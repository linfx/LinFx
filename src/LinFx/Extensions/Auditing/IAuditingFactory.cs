using System.Reflection;

namespace LinFx.Extensions.Auditing;

//TODO: Move ShouldSaveAudit & IsEntityHistoryEnabled and rename to IAuditingFactory
public interface IAuditingFactory
{
    /// <summary>
    /// 判断当前方法是否需要存储审计日志信息
    /// </summary>
    /// <param name="methodInfo">方法类型</param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

    bool IsEntityHistoryEnabled(Type entityType, bool defaultValue = false);

    /// <summary>
    /// 创建审计信息
    /// </summary>
    /// <returns></returns>
    AuditLogInfo CreateAuditLogInfo();

    /// <summary>
    /// 创建审计动作
    /// </summary>
    /// <param name="auditLog"></param>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    AuditLogActionInfo CreateAuditLogAction(AuditLogInfo auditLog, Type type, MethodInfo method, object[] arguments);

    /// <summary>
    /// 创建审计动作
    /// </summary>
    /// <param name="auditLog"></param>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    AuditLogActionInfo CreateAuditLogAction(AuditLogInfo auditLog, Type type, MethodInfo method, IDictionary<string, object> arguments);
}
