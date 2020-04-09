using LinFx.Data.Abstractions;
using LinFx.Domain.Models;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MediatR;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork
    {
        [Autowired]
        private readonly IAuditPropertySetter _auditPropertySetter;
        protected readonly IMediator _mediator;
        protected IDbContextTransaction _currentTransaction;
        protected readonly ServiceContext _context;
        protected readonly object ServiceProviderLock = new object();

        protected DbContext()
        {
            _auditPropertySetter = new AuditPropertySetter(null, null);
        }

        public DbContext([NotNull] DbContextOptions options) : base(options)
        {
            _auditPropertySetter = new AuditPropertySetter(null, null);
        }

        public DbContext([NotNull] DbContextOptions options, ServiceContext context) : base(options)
        {
            _context = context;
            LazyGetRequiredService(ref _auditPropertySetter);
        }

        public DbContext([NotNull] DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public DbContext(
            [NotNull] DbContextOptions options,
            IMediator mediator,
            IAuditPropertySetter auditPropertySetter)
            : this(options, mediator)
        {
            _auditPropertySetter = auditPropertySetter;
        }

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

        public async Task BeginTransactionAsync()
        {
            _currentTransaction ??= await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
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

        protected static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var entityTypes = typeToRegisters.Where(x => typeof(IEntity).IsAssignableFrom(x));
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }
    }
}
