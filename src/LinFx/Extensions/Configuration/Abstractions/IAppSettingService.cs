using System.Threading.Tasks;

namespace LinFx.Extensions.Configuration.Abstractions
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public interface IAppSettingService
    {
        Task<string> GetAsync(string key);

        Task<T> GetAsync<T>();

        Task<T> GetAsync<T>(string key);

        Task ClearCacheAsync(string key);
    }
}
