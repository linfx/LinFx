using LinFx.Extensions.Auditing;
using System.Threading.Tasks;

namespace LinFx.Extensions.AuditLogging
{
    public interface IAuditLogInfoToAuditLogConverter
    {
        Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo);
    }
}
