namespace LinFx.Extensions.EventBus.Local;

/// <summary>
/// 本地事件消息
/// </summary>
public class LocalEventMessage
{
    public Guid MessageId { get; }

    public object EventData { get; }

    public Type EventType { get; }

    public LocalEventMessage(Guid messageId, object eventData, Type eventType)
    {
        MessageId = messageId;
        EventData = eventData;
        EventType = eventType;
    }
}
