using LinFx.Data;
using LinFx.Domain.Entities;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MediatR;
using LinFx.Uow;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public abstract class DbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork
    {
        [Autowired]
        private readonly IAuditPropertySetter _auditPropertySetter = new AuditPropertySetter(null, null);
        protected readonly IMediator _mediator;
        protected IDbContextTransaction _currentTransaction;
        protected readonly ServiceContext _context;
        public IServiceProvider ServiceProvider { get; private set; }
        protected readonly object ServiceProviderLock = new object();

        public IDataFilter DataFilter { get; set; }

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_mediator != null)
            {
                // Dispatch Domain Events collection. 
                // Choices:
                // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
                // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
                // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
                // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
                await _mediator.DispatchDomainEventsAsync(this);
            }

            try
            {
                // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
                // performed through the DbContext will be committed
                OnBeforeSaveChanges();
                var result = await base.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected virtual void OnBeforeSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        _auditPropertySetter.SetCreationProperties(entry.Entity);
                        break;
                    case EntityState.Modified:
                        _auditPropertySetter.SetModificationProperties(entry.Entity);
                        break;
                    case EntityState.Deleted:
                        _auditPropertySetter.SetDeletionProperties(entry.Entity);
                        break;
                }
            }
        }

        /// <summary>
        /// 实体注册
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="typeToRegisters"></param>
        protected virtual void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var entityTypes = typeToRegisters.Where(x => typeof(IEntity).IsAssignableFrom(x));
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }
    }
}
