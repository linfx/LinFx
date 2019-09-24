using System.Threading;

namespace LinFx.Extensions.MultiTenancy
{
    [Service]
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