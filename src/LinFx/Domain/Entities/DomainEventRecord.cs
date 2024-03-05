namespace LinFx.Domain.Entities;

/// <summary>
/// 领域事件记录
/// </summary>
public class DomainEventRecord(object eventData, long eventOrder)
{
    public object EventData { get; } = eventData;

    public long EventOrder { get; } = eventOrder;
}
