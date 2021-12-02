using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow
{
    /// <summary>
    /// 数据库事务接口
    /// </summary>
    public interface ITransactionApi : IDisposable
    {
        Task CommitAsync();
    }
}