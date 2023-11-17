namespace LinFx.Domain.Entities.Events;

/// <summary>
/// ÁìÓòÊÂ¼ş
/// </summary>
[Serializable]
public class DomainEventEntry
{
    public object SourceEntity { get; }

    public object EventData { get; }

    public long EventOrder { get; }

    public DomainEventEntry(object sourceEntity, object eventData, long eventOrder)
    {
        SourceEntity = sourceEntity;
        EventData = eventData;
        EventOrder = eventOrder;
    }
}
