using System;

namespace LinFx.Extensions.Threading;

public interface IAmbientScopeProvider<T>
{
    /// <summary>
    /// 获取临时值
    /// </summary>
    /// <param name="contextKey">关键字</param>
    /// <returns></returns>
    T GetValue(string contextKey);

    /// <summary>
    /// 存储临时值
    /// </summary>
    /// <param name="contextKey">关键字</param>
    /// <param name="value">临时值</param>
    /// <returns></returns>
    IDisposable BeginScope(string contextKey, T value);
}
