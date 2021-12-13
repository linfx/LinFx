using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow
{
    [Service(ServiceLifetime.Singleton)]
    public class NullUnitOfWorkEventPublisher : IUnitOfWorkEventPublisher
    {
        public Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents)
        {
            return Task.CompletedTask;
        }

        public Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents)
        {
            return Task.CompletedTask;
        }
    }
}