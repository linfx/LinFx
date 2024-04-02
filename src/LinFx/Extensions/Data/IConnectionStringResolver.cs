namespace LinFx.Extensions.Data;

/// <summary>
/// 连接字符串解析器
/// </summary>
public interface IConnectionStringResolver
{
    Task<string> ResolveAsync(string? connectionStringName = null);
}
