using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace LinFx.Extensions.Auditing;

public class SimpleLogAuditingStore : IAuditingStore
{
    public ILogger Logger { get; set; }

    public SimpleLogAuditingStore()
    {
        Logger = NullLogger<SimpleLogAuditingStore>.Instance;
    }

    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        Logger.LogInformation(auditInfo.ToString());
        return Task.CompletedTask;
    }
}
