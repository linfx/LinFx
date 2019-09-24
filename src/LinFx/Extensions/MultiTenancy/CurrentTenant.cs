using System;

namespace LinFx.Extensions.MultiTenancy
{
    /// <summary>
    /// 当前租户
    /// </summary>
    [Service]
    public class CurrentTenant : ICurrentTenant
    {
        public virtual bool IsAvailable => !string.IsNullOrEmpty(Id);

        public virtual string Id => _currentTenantIdAccessor.Current?.Id;

        public string Name => _currentTenantIdAccessor.Current?.Name;

        private readonly ICurrentTenantAccessor _currentTenantIdAccessor;

        public CurrentTenant(ICurrentTenantAccessor currentTenantIdAccessor)
        {
            _currentTenantIdAccessor = currentTenantIdAccessor;
        }

        public IDisposable Change(string id, string name)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(string id, string name)
        {
            var parentScope = _currentTenantIdAccessor.Current;
            _currentTenantIdAccessor.Current = new TenantInfo(id, name);
            return new DisposeAction(() =>
            {
                _currentTenantIdAccessor.Current = parentScope;
            });
        }
    }
}