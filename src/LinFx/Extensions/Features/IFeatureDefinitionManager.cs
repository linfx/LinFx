using JetBrains.Annotations;

namespace LinFx.Extensions.Features;

/// <summary>
/// 特征定义管理器
/// </summary>
public interface IFeatureDefinitionManager
{
    /// <summary>
    /// 获取特征定义
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [NotNull]
    Task<FeatureDefinition> GetAsync([NotNull] string name);

    /// <summary>
    /// 获取特征定义
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<FeatureDefinition>> GetAllAsync();

    /// <summary>
    /// 获取特征定义
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<FeatureDefinition?> GetOrNullAsync(string name);

    /// <summary>
    /// 获取特征定义组
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync();
}
