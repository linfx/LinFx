using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace LinFx.Extensions.Auditing;

[Service(ServiceLifetime.Singleton)]
public class SimpleLogAuditingStore : IAuditingStore
{
    public ILogger<SimpleLogAuditingStore> Logger { get; set; }

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
