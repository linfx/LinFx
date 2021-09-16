using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace LinFx.Extensions.MultiTenancy
{
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class CurrentTenantAccessor : ICurrentTenantAccessor
    {
        private readonly AsyncLocal<TenantInfo> _current;

        public CurrentTenantAccessor()
        {
            _current = new AsyncLocal<TenantInfo>();
        }

        public TenantInfo Current
        {
            get => _current.Value;
            set => _current.Value = value;
        }
    }
}