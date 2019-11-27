using LinFx.Data.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace LinFx.Data
{
    public class DataFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _filters;

        public DataFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _filters = new ConcurrentDictionary<Type, object>();
        }
    }

    public class DataFilter<TFilter> : IDataFilter<TFilter> where TFilter : class
    {
        public bool IsEnabled
        {
            get
            {
                EnsureInitialized();
                return _filter.Value.IsEnabled;
            }
        }

        private readonly DataFilterOptions _options;

        private readonly AsyncLocal<DataFilterState> _filter;

        public DataFilter(IOptions<DataFilterOptions> options)
        {
            _options = options.Value;
            _filter = new AsyncLocal<DataFilterState>();
        }

        public IDisposable Enable()
        {
            if (IsEnabled)
            {
                return NullDisposable.Instance;
            }

            _filter.Value.IsEnabled = true;

            return new DisposeAction(() => Disable());
        }

        public IDisposable Disable()
        {
            if (!IsEnabled)
            {
                return NullDisposable.Instance;
            }

            _filter.Value.IsEnabled = false;

            return new DisposeAction(() => Enable());
        }

        private void EnsureInitialized()
        {
            if (_filter.Value != null)
            {
                return;
            }

            _filter.Value = _options.DefaultStates.GetOrDefault(typeof(TFilter))?.Clone() ?? new DataFilterState(true);
        }
    }
}