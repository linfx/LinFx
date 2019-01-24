using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);
        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
