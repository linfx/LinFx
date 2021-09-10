using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow
{
    public interface ITransactionApi : IDisposable
    {
        Task CommitAsync();
    }
}