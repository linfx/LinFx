using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore
{
    /// <summary>
    /// ���ݿ��������ṩ��
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<TDbContext> where TDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// ��ȡ���ݿ�������
        /// </summary>
        /// <returns></returns>
        Task<TDbContext> GetDbContextAsync();
    }
}
