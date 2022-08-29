using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.ObjectExtending;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.AuditLogging;

/// <summary>
/// 审计日志
/// </summary>
[DisableAuditing]
public class AuditLog : AggregateRoot<string>, IMultiTenant
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public virtual string ApplicationName { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public virtual string UserId { get; protected set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public virtual string UserName { get; protected set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public virtual string TenantId { get; protected set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public virtual string TenantName { get; protected set; }

    public virtual string ImpersonatorUserId { get; protected set; }

    public virtual string ImpersonatorTenantId { get; protected set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public virtual DateTime ExecutionTime { get; protected set; }

    public virtual int ExecutionDuration { get; protected set; }

    public virtual string ClientIpAddress { get; protected set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    public virtual string ClientName { get; protected set; }

    public virtual string ClientId { get; set; }

    public virtual string CorrelationId { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    public virtual string BrowserInfo { get; protected set; }

    public virtual string HttpMethod { get; protected set; }

    public virtual string Url { get; protected set; }

    public virtual string? Exceptions { get; protected set; }

    public virtual string? Comments { get; protected set; }

    public virtual int? HttpStatusCode { get; set; }

    /// <summary>
    /// 实体变化
    /// </summary>
    public virtual ICollection<EntityChange> EntityChanges { get; protected set; }

    /// <summary>
    /// 审计日志动作
    /// </summary>
    public virtual ICollection<AuditLogAction> Actions { get; protected set; }

    protected AuditLog() { }

    public AuditLog(
        string id,
        string applicationName,
        string tenantId,
        string tenantName,
        string userId,
        string userName,
        DateTime executionTime,
        int executionDuration,
        string clientIpAddress,
        string clientName,
        string clientId,
        string correlationId,
        string browserInfo,
        string httpMethod,
        string url,
        int? httpStatusCode,
        string impersonatorUserId,
        string impersonatorTenantId,
        ExtraPropertyDictionary extraPropertyDictionary,
        List<EntityChange> entityChanges,
        List<AuditLogAction> actions,
        string? exceptions,
        string? comments)
        : base(id)
    {
        ApplicationName = applicationName.Truncate(AuditLogConsts.MaxApplicationNameLength);
        TenantId = tenantId;
        TenantName = tenantName.Truncate(AuditLogConsts.MaxTenantNameLength);
        UserId = userId;
        UserName = userName.Truncate(AuditLogConsts.MaxUserNameLength);
        ExecutionTime = executionTime;
        ExecutionDuration = executionDuration;
        ClientIpAddress = clientIpAddress.Truncate(AuditLogConsts.MaxClientIpAddressLength);
        ClientName = clientName.Truncate(AuditLogConsts.MaxClientNameLength);
        ClientId = clientId.Truncate(AuditLogConsts.MaxClientIdLength);
        CorrelationId = correlationId.Truncate(AuditLogConsts.MaxCorrelationIdLength);
        BrowserInfo = browserInfo.Truncate(AuditLogConsts.MaxBrowserInfoLength);
        HttpMethod = httpMethod.Truncate(AuditLogConsts.MaxHttpMethodLength);
        Url = url.Truncate(AuditLogConsts.MaxUrlLength);
        HttpStatusCode = httpStatusCode;
        ImpersonatorUserId = impersonatorUserId;
        ImpersonatorTenantId = impersonatorTenantId;

        ExtraProperties = extraPropertyDictionary;
        EntityChanges = entityChanges;
        Actions = actions;
        Exceptions = exceptions;
        Comments = comments?.Truncate(AuditLogConsts.MaxCommentsLength);
    }
}
