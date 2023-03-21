using JetBrains.Annotations;

namespace LinFx.Extensions.MultiTenancy;

/// <summary>
/// 当前租户
/// </summary>
public interface ICurrentTenant
{
    bool IsAvailable { get; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [CanBeNull]
    string Id { get; }

    /// <summary>
    /// 租户名称
    /// </summary>
    [CanBeNull]
    string Name { get; }

    IDisposable Change(string id, string name = default);
}
