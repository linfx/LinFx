using LinFx.Extensions;
using System;
using System.Threading.Tasks;

namespace LinFx.Domain.Uow
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public string Id { get; }

        public IUnitOfWork Outer { get; set; }

        public UnitOfWorkOptions Options { get; }

        public bool IsDisposed => throw new NotImplementedException();

        public event EventHandler Completed;
        public event EventHandler Disposed;

        public void Begin(UnitOfWorkOptions options)
        {
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
            throw new NotImplementedException();
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new Exception("Complete is called before!");
            }

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
    }
}
