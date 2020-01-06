using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Mediator.Idempotency
{
    public class RequestManager : IRequestManager
    {
        public Task<bool> ExistAsync(Guid id)
        {
            return Task.FromResult(false);
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new Exception($"Request with {id} already exists") :
                new ClientRequest
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            //_context.Add(request);
            //await _context.SaveChangesAsync();
        }
    }
}
