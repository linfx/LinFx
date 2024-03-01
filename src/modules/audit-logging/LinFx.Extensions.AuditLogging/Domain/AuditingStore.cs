using LinFx.Extensions.Auditing;
using LinFx.Extensions.AuditLogging.EntityFrameworkCore;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Uow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 审计日志储存
/// </summary>
[Service]
public class AuditingStore(
    AuditLoggingDbContext dbContext,
    IUnitOfWorkManager unitOfWorkManager,
    IOptions<AuditingOptions> options,
    IAuditLogInfoToAuditLogConverter converter) : IAuditingStore
{
    public ILogger<AuditingStore> Logger { get; set; } = NullLogger<AuditingStore>.Instance;

    protected AuditLoggingDbContext AuditLoggingDbContext { get; } = dbContext;

    protected IUnitOfWorkManager UnitOfWorkManager { get; } = unitOfWorkManager;

    protected AuditingOptions Options { get; } = options.Value;

    protected IAuditLogInfoToAuditLogConverter Converter { get; } = converter;

    public virtual async Task SaveAsync(AuditLogInfo auditInfo)
    {
        if (!Options.HideErrors)
        {
            await SaveLogAsync(auditInfo);
            return;
        }

        try
        {
            await SaveLogAsync(auditInfo);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo.ToString());
            Logger.LogException(ex, LogLevel.Error);
        }
    }

    protected virtual async Task SaveLogAsync(AuditLogInfo auditInfo)
    {
        using var uow = UnitOfWorkManager.Begin(true);
        //await AuditLogRepository.Add(await Converter.ConvertAsync(auditInfo));
        await uow.CompleteAsync();
    }
}
