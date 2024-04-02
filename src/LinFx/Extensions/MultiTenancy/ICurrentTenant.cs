using System.Diagnostics.CodeAnalysis;

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
    [AllowNull]
    string Id { get; }

    /// <summary>
    /// 租户名称
    /// </summary>
    [AllowNull]
    string Name { get; }

    IDisposable Change(string? id, string? name = default);
}
