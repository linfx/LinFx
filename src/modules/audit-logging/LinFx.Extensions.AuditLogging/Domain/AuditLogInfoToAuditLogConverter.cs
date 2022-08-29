using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.Http;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;
using System.Text.Json;

namespace LinFx.Extensions.AuditLogging;

[Service]
public class AuditLogInfoToAuditLogConverter : IAuditLogInfoToAuditLogConverter
{
    protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; }

    public AuditLogInfoToAuditLogConverter(IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
    {
        ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
    }

    public virtual Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo)
    {
        var auditLogId = IDUtils.NewIdString();

        var extraProperties = new ExtraPropertyDictionary();
        if (auditLogInfo.ExtraProperties != null)
        {
            foreach (var pair in auditLogInfo.ExtraProperties)
            {
                extraProperties.Add(pair.Key, pair.Value);
            }
        }

        var entityChanges = auditLogInfo
                                .EntityChanges?
                                .Select(entityChangeInfo => new EntityChange(auditLogId, entityChangeInfo, tenantId: auditLogInfo.TenantId))
                                .ToList() ?? new List<EntityChange>();

        var actions = auditLogInfo
                          .Actions?
                          .Select(auditLogActionInfo => new AuditLogAction(IDUtils.NewIdString(), auditLogId, auditLogActionInfo, tenantId: auditLogInfo.TenantId))
                          .ToList() ?? new List<AuditLogAction>();

        var remoteServiceErrorInfos = auditLogInfo.Exceptions?.Select(exception => ExceptionToErrorInfoConverter.Convert(exception)) ?? new List<RemoteServiceErrorInfo>();
        var exceptions = remoteServiceErrorInfos.Any()
            ? JsonSerializer.Serialize(remoteServiceErrorInfos)
            : null;

        var comments = auditLogInfo
            .Comments?
            .JoinAsString(Environment.NewLine);

        var auditLog = new AuditLog(
            auditLogId,
            auditLogInfo.ApplicationName,
            auditLogInfo.TenantId,
            auditLogInfo.TenantName,
            auditLogInfo.UserId,
            auditLogInfo.UserName,
            auditLogInfo.ExecutionTime,
            auditLogInfo.ExecutionDuration,
            auditLogInfo.ClientIpAddress,
            auditLogInfo.ClientName,
            auditLogInfo.ClientId,
            auditLogInfo.CorrelationId,
            auditLogInfo.BrowserInfo,
            auditLogInfo.HttpMethod,
            auditLogInfo.Url,
            auditLogInfo.HttpStatusCode,
            auditLogInfo.ImpersonatorUserId,
            auditLogInfo.ImpersonatorTenantId,
            extraProperties,
            entityChanges,
            actions,
            exceptions,
            comments
        );

        return Task.FromResult(auditLog);
    }
}
