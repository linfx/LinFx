namespace LinFx.Extensions.Uow;

/// <summary>
/// 领域事件记录
/// </summary>
public class UnitOfWorkEventRecord(Type eventType, object eventData, long eventOrder)
{
    /// <summary>
    /// 事件数据
    /// </summary>
    public object EventData { get; } = eventData;

    /// <summary>
    /// 事件类型
    /// </summary>
    public Type EventType { get; } = eventType;

    /// <summary>
    /// 事件顺序
    /// </summary>
    public long EventOrder { get; } = eventOrder;

    /// <summary>
    /// Extra properties can be used if needed.
    /// </summary>
    public Dictionary<string, object> Properties { get; } = [];
}
