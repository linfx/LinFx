namespace LinFx.Extensions.EventBus.Local;

/// <summary>
/// 本地事件消息
/// </summary>
public class LocalEventMessage(Guid messageId, object eventData, Type eventType)
{
    public Guid MessageId { get; } = messageId;

    public object EventData { get; } = eventData;

    public Type EventType { get; } = eventType;
}
