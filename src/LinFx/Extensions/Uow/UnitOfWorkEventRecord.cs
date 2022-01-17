using System;
using System.Collections.Generic;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 领域事件记录
/// </summary>
public class UnitOfWorkEventRecord
{
    /// <summary>
    /// 事件数据
    /// </summary>
    public object EventData { get; }

    /// <summary>
    /// 事件类型
    /// </summary>
    public Type EventType { get; }

    /// <summary>
    /// 事件顺序
    /// </summary>
    public long EventOrder { get; }

    /// <summary>
    /// Extra properties can be used if needed.
    /// </summary>
    public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

    public UnitOfWorkEventRecord(Type eventType, object eventData, long eventOrder)
    {
        EventType = eventType;
        EventData = eventData;
        EventOrder = eventOrder;
    }
}
