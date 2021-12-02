﻿using Microsoft.Extensions.Options;
using System;

namespace LinFx.Extensions.Timing;

/// <summary>
/// 时钟
/// </summary>
[Service]
public class Clock : IClock
{
    protected ClockOptions Options { get; }

    public Clock(IOptions<ClockOptions> options)
    {
        Options = options.Value;
    }

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