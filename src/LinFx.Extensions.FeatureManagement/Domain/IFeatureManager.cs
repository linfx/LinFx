﻿using JetBrains.Annotations;

namespace LinFx.Extensions.FeatureManagement;

/// <summary>
/// 特征管理器
/// </summary>
public interface IFeatureManager
{
    Task<string?> GetOrNullAsync([NotNull] string name, [NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    Task<List<FeatureNameValue>> GetAllAsync([NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderAsync([NotNull] string name, [NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderAsync([NotNull] string providerName, [CanBeNull] string providerKey, bool fallback = true);

    Task SetAsync([NotNull] string name, [CanBeNull] string value, [NotNull] string providerName, [CanBeNull] string providerKey, bool forceToSet = false);

    Task DeleteAsync(string providerName, string providerKey);
}
