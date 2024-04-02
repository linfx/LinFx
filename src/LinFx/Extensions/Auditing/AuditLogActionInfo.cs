using LinFx.Extensions.ObjectExtending;

namespace LinFx.Extensions.Auditing;

/// <summary>
/// 执行的动作
/// </summary>
[Serializable]
public class AuditLogActionInfo : IHasExtraProperties
{
    /// <summary>
    /// :执行的控制器/服务的名称.
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// 控制器/服务执行的方法的名称.
    /// </summary>
    public string MethodName { get; set; }

    /// <summary>
    /// 控制器/服务执行的方法的名称.
    /// </summary>
    public string Parameters { get; set; }

    /// <summary>
    /// 执行的时间.
    /// </summary>
    public DateTimeOffset ExecutionTime { get; set; }

    /// <summary>
    /// 方法执行时长（单位：毫秒）. 
    /// </summary>
    public int ExecutionDuration { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; } = [];
}
