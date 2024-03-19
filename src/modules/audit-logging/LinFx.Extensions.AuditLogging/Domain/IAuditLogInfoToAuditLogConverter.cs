using LinFx.Extensions.Auditing;

namespace LinFx.Extensions.AuditLogging;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo);
}
