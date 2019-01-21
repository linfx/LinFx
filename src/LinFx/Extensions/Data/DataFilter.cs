using System;
using System.Collections.Concurrent;

namespace LinFx.Extensions.Data
{
    public class DataFilter
    {
        private readonly ConcurrentDictionary<Type, object> _filters;

        private readonly IServiceProvider _serviceProvider;

        public DataFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _filters = new ConcurrentDictionary<Type, object>();
        }
    }
}