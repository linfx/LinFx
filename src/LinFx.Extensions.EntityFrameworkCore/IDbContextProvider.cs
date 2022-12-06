using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore;

/// <summary>
/// 数据库上下文提供者
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <returns></returns>
    Task<TDbContext> GetDbContextAsync();
}
