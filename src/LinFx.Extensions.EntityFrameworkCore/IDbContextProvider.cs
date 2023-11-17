using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore;

/// <summary>
/// ���ݿ��������ṩ��
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// ��ȡ���ݿ�������
    /// </summary>
    /// <returns></returns>
    Task<TDbContext> GetDbContextAsync();
}
