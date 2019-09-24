using LinFx.Domain.Models;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using LinFx.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinFx.Application
{
    /// <summary>
    /// 服务抽象类
    /// </summary>
    public abstract class ApplicationService
    {
        private ILoggerFactory _loggerFactory;
        private ICurrentTenant _currentTenant;
        private IMediator _mediator;
        protected readonly ServiceContext _context;
        protected readonly object ServiceProviderLock = new object();

        protected ApplicationService(ServiceContext context)
        {
            _context = context;
        }

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = _context.ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }
            return reference;
        }

        public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

        public ICurrentTenant CurrentTenant => LazyGetRequiredService(ref _currentTenant);

        public IMediator Mediator => LazyGetRequiredService(ref _mediator);

        protected virtual void SetId<TEntity>(TEntity entity)
        {
            if (entity is IEntity<string> entityWithStringId)
            {
                if (string.IsNullOrWhiteSpace(entityWithStringId.Id))
                    entityWithStringId.Id = IDUtils.NewId().ToString();
            }
        }

        protected virtual void TryToSetTenantId<TEntity>(TEntity entity)
        {
            if (entity is IMultiTenant && HasTenantIdProperty(entity))
            {
                var tenantId = CurrentTenant.Id;

                if (string.IsNullOrEmpty(tenantId))
                {
                    return;
                }

                var propertyInfo = entity.GetType().GetProperty(nameof(IMultiTenant.TenantId));

                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(entity, tenantId, null);
                }
            }
        }

        protected virtual bool HasTenantIdProperty<TEntity>(TEntity entity)
        {
            return entity.GetType().GetProperty(nameof(IMultiTenant.TenantId)) != null;
        }
    }
}
