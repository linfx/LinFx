using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Domain.Uow
{
    public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// Unique id of this UOW.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Reference to the outer UOW if exists.
        /// </summary>
        IUnitOfWork Outer { get; set; }
        /// <summary>
        /// Begins the unit of work with given options.
        /// </summary>
        /// <param name="options">Unit of work options</param>
        void Begin(UnitOfWorkOptions options);
    }

	public interface IActiveUnitOfWork
	{
		/// <summary>
		/// This event is raised when this UOW is successfully completed.
		/// </summary>
		event EventHandler Completed;
		/// <summary>
		/// This event is raised when this UOW is failed.
		/// </summary>
		event EventHandler<UnitOfWorkFailedEventArgs> Failed;
		/// <summary>
		/// This event is raised when this UOW is disposed.
		/// </summary>
		event EventHandler Disposed;
		/// <summary>
		/// Gets if this unit of work is transactional.
		/// </summary>
		UnitOfWorkOptions Options { get; }
		/// <summary>
		/// Gets data filter configurations for this unit of work.
		/// </summary>
		IReadOnlyList<DataFilterConfiguration> Filters { get; }
		/// <summary>
		/// Is this UOW disposed?
		/// </summary>
		bool IsDisposed { get; }
		/// <summary>
		/// Saves all changes until now in this unit of work.
		/// This method may be called to apply changes whenever needed.
		/// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
		/// No explicit call is needed to SaveChanges generally, 
		/// since all changes saved at end of a unit of work automatically.
		/// </summary>
		void SaveChanges();
		/// <summary>
		/// Saves all changes until now in this unit of work.
		/// This method may be called to apply changes whenever needed.
		/// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
		/// No explicit call is needed to SaveChanges generally, 
		/// since all changes saved at end of a unit of work automatically.
		/// </summary>
		Task SaveChangesAsync();
		/// <summary>
		/// Disables one or more data filters.
		/// Does nothing for a filter if it's already disabled. 
		/// Use this method in a using statement to re-enable filters if needed.
		/// </summary>
		/// <param name="filterNames">One or more filter names for standard filters.</param>
		/// <returns>A <see cref="IDisposable"/> handle to take back the disable effect.</returns>
		IDisposable DisableFilter(params string[] filterNames);
		/// <summary>
		/// Enables one or more data filters.
		/// Does nothing for a filter if it's already enabled.
		/// Use this method in a using statement to re-disable filters if needed.
		/// </summary>
		/// <param name="filterNames">One or more filter names for standard filters.</param>
		/// <returns>A <see cref="IDisposable"/> handle to take back the enable effect.</returns>
		IDisposable EnableFilter(params string[] filterNames);
		/// <summary>
		/// Checks if a filter is enabled or not.
		/// </summary>
		/// <param name="filterName">Name of the filter for standard filters.</param>
		bool IsFilterEnabled(string filterName);
		/// <summary>
		/// Sets (overrides) value of a filter parameter.
		/// </summary>
		/// <param name="filterName">Name of the filter</param>
		/// <param name="parameterName">Parameter's name</param>
		/// <param name="value">Value of the parameter to be set</param>
		IDisposable SetFilterParameter(string filterName, string parameterName, object value);
	}

	/// <summary>
	/// Used to get/set current <see cref="IUnitOfWork"/>. 
	/// </summary>
	public interface ICurrentUnitOfWorkProvider
	{
		/// <summary>
		/// Gets/sets current <see cref="IUnitOfWork"/>.
		/// </summary>
		IUnitOfWork Current { get; set; }
	}

	public interface IUnitOfWorkFilterExecuter
	{
		void ApplyDisableFilter(IUnitOfWork unitOfWork, string filterName);
		void ApplyEnableFilter(IUnitOfWork unitOfWork, string filterName);
		void ApplyFilterParameterValue(IUnitOfWork unitOfWork, string filterName, string parameterName, object value);
	}

	/// <summary>
	/// Used as event arguments on <see cref="IActiveUnitOfWork.Failed"/> event.
	/// </summary>
	public class UnitOfWorkFailedEventArgs : EventArgs
	{
		/// <summary>
		/// Exception that caused failure.
		/// </summary>
		public Exception Exception { get; private set; }
		/// <summary>
		/// Creates a new <see cref="UnitOfWorkFailedEventArgs"/> object.
		/// </summary>
		/// <param name="exception">Exception that caused failure</param>
		public UnitOfWorkFailedEventArgs(Exception exception)
		{
			Exception = exception;
		}
	}
}