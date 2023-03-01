namespace LinFx.Domain.Entities;

/// <summary>
/// 领域事件记录
/// </summary>
public class DomainEventRecord
{
    public object EventData { get; }

    public long EventOrder { get; }

    public DomainEventRecord(object eventData, long eventOrder)
    {
        EventData = eventData;
        EventOrder = eventOrder;
    }
}
