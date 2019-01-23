using System.Threading.Tasks;

namespace LinFx.Extensions.Auditing
{
    public interface IAuditingStore
    {
        Task SaveAsync(AuditLogInfo auditInfo);
    }
}