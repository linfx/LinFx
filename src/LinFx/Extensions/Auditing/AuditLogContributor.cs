namespace LinFx.Extensions.Auditing;

/// <summary>
/// 审计日志贡献者
/// </summary>
public abstract class AuditLogContributor
{
    public virtual void PreContribute(AuditLogContributionContext context)
    {
    }

    public virtual void PostContribute(AuditLogContributionContext context)
    {
    }
}
