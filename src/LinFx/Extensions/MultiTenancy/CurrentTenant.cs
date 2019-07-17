using System;

namespace LinFx.Extensions.MultiTenancy
{
    /// <summary>
    /// 当前租户
    /// </summary>
    public class CurrentTenant : ICurrentTenant
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual Guid? Id => _currentTenantIdAccessor.Current?.Id;

        public string Name => _currentTenantIdAccessor.Current?.Name;

        private readonly ICurrentTenantIdAccessor _currentTenantIdAccessor;

        public CurrentTenant(ICurrentTenantIdAccessor currentTenantIdAccessor)
        {
            _currentTenantIdAccessor = currentTenantIdAccessor;
        }

        public IDisposable Change(Guid? id, string name)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(Guid? id, string name)
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