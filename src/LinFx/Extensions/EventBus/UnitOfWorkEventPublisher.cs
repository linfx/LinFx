using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;
using LinFx.Extensions.Uow;

namespace LinFx.Extensions.EventBus;

/// <summary>
/// 领域事件发布器
/// </summary>
/// <param name="localEventBus"></param>
/// <param name="distributedEventBus"></param>
public class UnitOfWorkEventPublisher(ILocalEventBus localEventBus, IDistributedEventBus distributedEventBus) : IUnitOfWorkEventPublisher
{
    private readonly ILocalEventBus _localEventBus = localEventBus;
    private readonly IDistributedEventBus _distributedEventBus = distributedEventBus;

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
