using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;
using LinFx.Extensions.Uow;

namespace LinFx.Extensions.EventBus;

public class UnitOfWorkEventPublisher : IUnitOfWorkEventPublisher
{
    private readonly ILocalEventBus _localEventBus;
    private readonly IDistributedEventBus _distributedEventBus;

    public UnitOfWorkEventPublisher(
        ILocalEventBus localEventBus,
        IDistributedEventBus distributedEventBus)
    {
        _localEventBus = localEventBus;
        _distributedEventBus = distributedEventBus;
    }

    public async Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents)
    {
        foreach (var localEvent in localEvents)
        {
            await _localEventBus.PublishAsync(localEvent.EventType, localEvent.EventData, onUnitOfWorkComplete: false);
        }
    }

    public async Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents)
    {
        foreach (var distributedEvent in distributedEvents)
        {
            await _distributedEventBus.PublishAsync(distributedEvent.EventType, distributedEvent.EventData, onUnitOfWorkComplete: false);
        }
    }
}
