using System.Threading.Tasks;

namespace LinFx.Domain.Models.Auditing
{
    public interface IAuditingStore
    {
        Task SaveAsync(AuditLogInfo auditInfo);
    }
}