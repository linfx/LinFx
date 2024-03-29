using LinFx.Data;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LinFx.Extensions.Data;

/// <summary>
/// 数据过滤
/// </summary>
public class DataFilter(IServiceProvider serviceProvider) : IDataFilter, ISingletonDependency
{
    private readonly ConcurrentDictionary<Type, object> _filters = new();

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IDisposable Enable<TFilter>() where TFilter : class => GetFilter<TFilter>().Enable();

    public IDisposable Disable<TFilter>() where TFilter : class => GetFilter<TFilter>().Disable();

    public bool IsEnabled<TFilter>() where TFilter : class => GetFilter<TFilter>().IsEnabled;

    private IDataFilter<TFilter> GetFilter<TFilter>() where TFilter : class => (_filters.GetOrAdd(typeof(TFilter), _serviceProvider.GetRequiredService<IDataFilter<TFilter>>) as IDataFilter<TFilter>)!;
}

public class DataFilter<TFilter>(IOptions<DataFilterOptions> options) : IDataFilter<TFilter> where TFilter : class
{
    private readonly DataFilterOptions _options = options.Value;
    private readonly AsyncLocal<DataFilterState> _filter = new AsyncLocal<DataFilterState>();

    public bool IsEnabled
    {
        get
        {
            EnsureInitialized();
            return _filter.Value.IsEnabled;
        }
    }

    public IDisposable Enable()
    {
        if (IsEnabled)
            return NullDisposable.Instance;

        _filter.Value.IsEnabled = true;

        return new DisposeAction(() => Disable());
    }

    public IDisposable Disable()
    {
        if (!IsEnabled)
            return NullDisposable.Instance;

        _filter.Value.IsEnabled = false;

        return new DisposeAction(() => Enable());
    }

    private void EnsureInitialized()
    {
        if (_filter.Value != null)
            return;

        _filter.Value = _options.DefaultStates.GetOrDefault(typeof(TFilter))?.Clone() ?? new DataFilterState(true);
    }
}
