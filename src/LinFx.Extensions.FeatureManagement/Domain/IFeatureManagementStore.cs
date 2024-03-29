namespace LinFx.Extensions.FeatureManagement;

public interface IFeatureManagementStore
{
    /// <summary>
    /// 获取特征值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<string?> GetOrNullAsync(string name, string providerName, string providerKey);

    Task SetAsync(string name, string value, string providerName, string providerKey);

    Task DeleteAsync(string name, string providerName, string providerKey);
}
