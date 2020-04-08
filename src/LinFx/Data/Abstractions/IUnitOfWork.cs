using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Data.Abstractions
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
