using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Mediator.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}