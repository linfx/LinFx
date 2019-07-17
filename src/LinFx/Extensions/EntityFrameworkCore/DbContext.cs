using LinFx.Domain.Abstractions;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork
    {
        private readonly IAuditPropertySetter _auditPropertySetter = new AuditPropertySetter();
        protected readonly IMediator _mediator;
        protected IDbContextTransaction _currentTransaction;

        protected DbContext() { }

        public DbContext([NotNull] DbContextOptions options) : base(options) { }

        public DbContext([NotNull] DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public IDataFilter DataFilter { get; set; }

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

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = _currentTransaction ?? await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await base.SaveChangesAsync();
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
    }
}
