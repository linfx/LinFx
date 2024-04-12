using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.Timing;

/// <summary>
/// 系统时钟
/// </summary>
public class Clock(IOptions<ClockOptions> options) : IClock, ITransientDependency
{
    protected ClockOptions Options { get; } = options.Value;

    /// <summary>
    /// 当前时间
    /// </summary>
    public virtual DateTime Now => Options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;

    /// <summary>
    /// 当前使用的时钟类型
    /// </summary>
    public virtual DateTimeKind Kind => Options.Kind;

    /// <summary>
    /// 如果当前时间是UTC,则返回 true.
    /// </summary>
    public virtual bool SupportsMultipleTimezone => Options.Kind == DateTimeKind.Utc;

    /// <summary>
    /// DateTime 标准化
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public virtual DateTime Normalize(DateTime dateTime)
    {
        if (Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind)
            return dateTime;

        if (Kind == DateTimeKind.Local && dateTime.Kind == DateTimeKind.Utc)
            return dateTime.ToLocalTime();

        if (Kind == DateTimeKind.Utc && dateTime.Kind == DateTimeKind.Local)
            return dateTime.ToUniversalTime();

        return DateTime.SpecifyKind(dateTime, Kind);
    }
}
