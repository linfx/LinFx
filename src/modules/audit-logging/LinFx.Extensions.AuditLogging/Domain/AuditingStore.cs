using LinFx.Extensions.Auditing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.AuditLogging.Domain
{
    public class AuditingStore
    {
        public ILogger<AuditingStore> Logger { get; set; }

        protected IAuditLogRepository AuditLogRepository { get; }

        public virtual async Task SaveAsync(AuditLogInfo auditInfo)
        {
            //if (!Options.HideErrors)
            //{
            //    await SaveLogAsync(auditInfo);
            //    return;
            //}

            //try
            //{
            //    await SaveLogAsync(auditInfo);
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo.ToString());
            //    Logger.LogException(ex, LogLevel.Error);
            //}
        }

        protected virtual async Task SaveLogAsync(AuditLogInfo auditInfo)
        {
            //using (var uow = UnitOfWorkManager.Begin(true))
            //{
            //    await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, auditInfo));
            //    await uow.SaveChangesAsync();
            //}
        }
    }
}
