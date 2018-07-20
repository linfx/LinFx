using System;
using System.Transactions;

namespace LinFx.Domain.Uow
{
    /// <summary>
    /// Unit of work manager.
    /// Used to begin and control a unit of work.
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Gets currently active unit of work (or null if not exists).
        /// </summary>
        IActiveUnitOfWork Current { get; }
        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWorkCompleteHandle Begin();
        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);
        /// <summary>
        /// Begins a new unit of work.
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work</returns>
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }

    public class UnitOfWorkManager : IUnitOfWorkManager
    {
		private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
		private readonly IUnitOfWorkDefaultOptions _defaultOptions;

		public UnitOfWorkManager(
			ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
			IUnitOfWorkDefaultOptions defaultOptions)
		{
			_currentUnitOfWorkProvider = currentUnitOfWorkProvider;
			_defaultOptions = defaultOptions;
		}

		public IActiveUnitOfWork Current
        {
            get { return _currentUnitOfWorkProvider.Current; }
        }

		public IUnitOfWorkCompleteHandle Begin()
		{
			return Begin(new UnitOfWorkOptions());
		}

		public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
		{
			return Begin(new UnitOfWorkOptions { Scope = scope });
		}

		public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
		{
            throw new NotImplementedException();
		}
	}
}
