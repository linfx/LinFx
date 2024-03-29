using JetBrains.Annotations;

namespace LinFx.Extensions.FeatureManagement;

/// <summary>
/// 特征管理器
/// </summary>
public interface IFeatureManager
{
    Task<string?> GetOrNullAsync([NotNull] string name, [NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    Task<List<FeatureNameValue>> GetAllAsync([NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    /// <summary>
    /// 获取特征值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderAsync([NotNull] string name, [NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    /// <summary>
    /// 获取特征值
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderAsync([NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    /// <summary>
    /// 更新特征值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="forceToSet"></param>
    /// <returns></returns>
    Task SetAsync([NotNull] string name, [CanBeNull] string value, [NotNull] string providerName, [CanBeNull] string providerKey, bool forceToSet = false);

    /// <summary>
    /// 删除特征
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task DeleteAsync(string providerName, string providerKey);
}
