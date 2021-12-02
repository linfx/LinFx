using LinFx.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计贡献者上下文
/// </summary>
public class AuditLogContributionContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 审计信息
    /// </summary>
    public AuditLogInfo AuditInfo { get; }

    public AuditLogContributionContext(IServiceProvider serviceProvider, AuditLogInfo auditInfo)
    {
        ServiceProvider = serviceProvider;
        AuditInfo = auditInfo;
    }
}
