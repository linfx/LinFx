using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;
using LinFx.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LinFx.Extensions.Threading;

/// <summary>
/// 周围上下文
/// </summary>
/// <typeparam name="T"></typeparam>
public class AmbientDataContextAmbientScopeProvider<T> : IAmbientScopeProvider<T>
{
    public ILogger<AmbientDataContextAmbientScopeProvider<T>> Logger { get; set; }

    private static readonly ConcurrentDictionary<string, ScopeItem> ScopeDictionary = new();

    private readonly IAmbientDataContext _dataContext;

    public AmbientDataContextAmbientScopeProvider([NotNull] IAmbientDataContext dataContext)
    {
        Check.NotNull(dataContext, nameof(dataContext));

        _dataContext = dataContext;
        Logger = NullLogger<AmbientDataContextAmbientScopeProvider<T>>.Instance;
    }

    public T GetValue(string contextKey)
    {
        var item = GetCurrentItem(contextKey);
        if (item == null)
            return default;

        return item.Value;
    }

    public IDisposable BeginScope(string contextKey, T value)
    {
        // 将需要临时存储的对象，用 ScopeItem 包装起来，它的外部对象是当前对象 (如果存在的话)。
        var item = new ScopeItem(value, GetCurrentItem(contextKey));

        // 将包装好的对象以 Id-对象，的形式存储在字典当中。
        if (!ScopeDictionary.TryAdd(item.Id, item))
            throw new Exception("Can not add item! ScopeDictionary.TryAdd returns false!");

        // 在上下文当中设置当前的 ContextKey 关联的 Id。
        _dataContext.SetData(contextKey, item.Id);

        // 集合释放委托，using 语句块结束时，做释放操作。
        return new DisposeAction(() =>
        {
            // 从字典中移除指定 Id 的对象。
            ScopeDictionary.TryRemove(item.Id, out item);

            // 如果包装对象没有外部对象，直接设置上下文关联的 Id 为 NULL。
            if (item.Outer == null)
            {
                _dataContext.SetData(contextKey, null);
                return;
            }

            // 如果包装对象没有外部对象，直接设置上下文关联的 Id 为 NULL。
            _dataContext.SetData(contextKey, item.Outer.Id);
        });
    }

    private ScopeItem GetCurrentItem(string contextKey)
    {
        // 如果包装对象没有外部对象，直接设置上下文关联的 Id 为 NULL。
        // 不存在则返回 NULL，存在则尝试以 Id 从字典中拿取对象外部，并返回。
        return _dataContext.GetData(contextKey) is string objKey ? ScopeDictionary.GetOrDefault(objKey) : null;
    }

    private class ScopeItem
    {
        public string Id { get; }

        public ScopeItem Outer { get; }

        public T Value { get; }

        public ScopeItem(T value, ScopeItem outer = null)
        {
            Id = Guid.NewGuid().ToString();

            Value = value;
            Outer = outer;
        }
    }
}
