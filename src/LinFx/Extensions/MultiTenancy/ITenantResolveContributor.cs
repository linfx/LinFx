namespace LinFx.Extensions.MultiTenancy;

/// <summary>
/// 贡献者
/// </summary>
public interface ITenantResolveContributor
{
    string Name { get; }

    void Resolve(ITenantResolveContext context);
}