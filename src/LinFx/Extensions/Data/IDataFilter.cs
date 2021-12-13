using System;

namespace LinFx.Extensions.Data;

/// <summary>
/// 数据过滤
/// </summary>
public interface IDataFilter
{
    /// <summary>
    /// 启用
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    /// <returns></returns>
    IDisposable Enable<TFilter>()
        where TFilter : class;

    /// <summary>
    /// 关闭
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    /// <returns></returns>
    IDisposable Disable<TFilter>()
        where TFilter : class;

    /// <summary>
    /// 是否启用
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    /// <returns></returns>
    bool IsEnabled<TFilter>()
        where TFilter : class;
}

/// <summary>
/// 数据过滤
/// </summary>
/// <typeparam name="TFilter"></typeparam>
public interface IDataFilter<TFilter>
    where TFilter : class
{
    /// <summary>
    /// 启用
    /// </summary>
    /// <returns></returns>
    IDisposable Enable();

    /// <summary>
    /// 关闭
    /// </summary>
    /// <returns></returns>
    IDisposable Disable();

    /// <summary>
    /// 是否启用
    /// </summary>
    bool IsEnabled { get; }
}
