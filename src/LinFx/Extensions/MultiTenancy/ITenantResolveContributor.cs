namespace LinFx.Extensions.MultiTenancy;

/// <summary>
/// 租户解析贡献者
/// </summary>
public interface ITenantResolveContributor
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="context"></param>
    void Resolve(ITenantResolveContext context);
}