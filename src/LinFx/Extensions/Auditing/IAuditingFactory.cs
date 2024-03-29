using System.Reflection;

namespace LinFx.Extensions.Auditing;

//TODO: Move ShouldSaveAudit & IsEntityHistoryEnabled and rename to IAuditingFactory
public interface IAuditingFactory
{
    /// <summary>
    /// �жϵ�ǰ�����Ƿ���Ҫ�洢�����־��Ϣ
    /// </summary>
    /// <param name="methodInfo">��������</param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

    bool IsEntityHistoryEnabled(Type entityType, bool defaultValue = false);

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    /// <returns></returns>
    AuditLogInfo CreateAuditLogInfo();

    /// <summary>
    /// ������ƶ���
    /// </summary>
    /// <param name="auditLog"></param>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    AuditLogActionInfo CreateAuditLogAction(AuditLogInfo auditLog, Type type, MethodInfo method, object[] arguments);

    /// <summary>
    /// ������ƶ���
    /// </summary>
    /// <param name="auditLog"></param>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    AuditLogActionInfo CreateAuditLogAction(AuditLogInfo auditLog, Type type, MethodInfo method, IDictionary<string, object> arguments);
}
