using LinFx.Extensions.DependencyInjection;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计贡献者上下文
/// </summary>
public class AuditLogContributionContext(IServiceProvider serviceProvider, AuditLogInfo auditInfo) : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// 审计信息
    /// </summary>
    public AuditLogInfo AuditInfo { get; } = auditInfo;
}
