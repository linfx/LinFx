using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Uow;

[Service(ServiceLifetime.Singleton)]
public class NullUnitOfWorkEventPublisher : IUnitOfWorkEventPublisher
{
    public Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents) => Task.CompletedTask;

    public Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents) => Task.CompletedTask;
}