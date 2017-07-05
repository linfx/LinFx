using LinFx.Extensions;
using LinFx.Session;
using System;
using System.Threading.Tasks;

namespace LinFx.Domain.Uow
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public string Id { get; }

        public IUnitOfWork Outer { get; set; }
        /// <summary>
        /// Reference to current session.
        /// </summary>
        public ISession Session { protected get; set; }

        public UnitOfWorkOptions Options { get; }

        public bool IsDisposed => throw new NotImplementedException();

        public event EventHandler Completed;
        public event EventHandler Disposed;

        public void Begin(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            //SetFilters(options.FilterOverrides);

            //SetTenantId(Session.TenantId, false);

            BeginUow();
        }

        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public void Dispose()
        {
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
                throw new LinFxException("Complete is called before!");

            _isCompleteCalledBefore = true;
        }

        /// <summary>
        /// Is <see cref="Begin"/> method called before?
        /// </summary>
        private bool _isBeginCalledBefore;

        /// <summary>
        /// Is <see cref="Complete"/> method called before?
        /// </summary>
        private bool _isCompleteCalledBefore;

        /// <summary>
        /// Is this unit of work successfully completed.
        /// </summary>
        private bool _succeed;

        /// <summary>
        /// A reference to the exception if this unit of work failed.
        /// </summary>
        private Exception _exception;

        /// <summary>
        /// Should be implemented by derived classes to start UOW.
        /// </summary>
        protected abstract void BeginUow();
        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();
        /// <summary>
        /// Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();
        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();

        /// <summary>
        /// Called to trigger <see cref="Completed"/> event.
        /// </summary>
        protected virtual void OnCompleted() => Completed.InvokeSafely(this);

        public abstract void SaveChanges();

        public abstract Task SaveChangesAsync();

        //public virtual IDisposable SetTenantId(int? tenantId, bool switchMustHaveTenantEnableDisable)
        //{
        //    //var oldTenantId = _tenantId;
        //    //_tenantId = tenantId;

        //    IDisposable mustHaveTenantEnableChange;
        //    if (switchMustHaveTenantEnableDisable)
        //    {
        //        mustHaveTenantEnableChange = tenantId == null
        //            ? DisableFilter(AbpDataFilters.MustHaveTenant)
        //            : EnableFilter(AbpDataFilters.MustHaveTenant);
        //    }
        //    else
        //    {
        //        mustHaveTenantEnableChange = NullDisposable.Instance;
        //    }

        //    var mayHaveTenantChange = SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId);
        //    var mustHaveTenantChange = SetFilterParameter(AbpDataFilters.MustHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId ?? 0);

        //    return new DisposeAction(() =>
        //    {
        //        mayHaveTenantChange.Dispose();
        //        mustHaveTenantChange.Dispose();
        //        mustHaveTenantEnableChange.Dispose();
        //        _tenantId = oldTenantId;
        //    });
        //}
    }
}
