using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.MediatR.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}