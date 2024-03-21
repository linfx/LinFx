using JetBrains.Annotations;
using LinFx.Extensions.Features;

namespace LinFx.Extensions.FeatureManagement;

public interface IFeatureManagementProvider
{
    string Name { get; }

    //TODO: Other better method name.
    bool Compatible(string providerName);

    //TODO: Other better method name.
    Task<IAsyncDisposable> HandleContextAsync(string providerName, string providerKey);

    /// <summary>
    /// 获取特征值
    /// </summary>
    /// <param name="feature"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<string?> GetOrNullAsync([NotNull] FeatureDefinition feature, [CanBeNull] string providerKey);

    Task SetAsync([NotNull] FeatureDefinition feature, [NotNull] string value, [CanBeNull] string providerKey);

    Task ClearAsync([NotNull] FeatureDefinition feature, [CanBeNull] string providerKey);
}
