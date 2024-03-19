using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LinFx.Extensions.Auditing;

public class SimpleLogAuditingStore : IAuditingStore
{
    public ILogger Logger { get; set; } = NullLogger<SimpleLogAuditingStore>.Instance;

    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        Logger.LogInformation(auditInfo.ToString());
        return Task.CompletedTask;
    }
}
