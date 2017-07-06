using System;
using System.Collections.Generic;
using System.Transactions;

namespace LinFx.Domain.Uow
{
	/// <summary>
	/// Used to get/set default options for a unit of work.
	/// </summary>
	public interface IUnitOfWorkDefaultOptions
	{
	}

	public class UnitOfWorkOptions
    {
		/// <summary>
		/// Scope option.
		/// </summary>
		public TransactionScopeOption? Scope { get; set; }
		/// <summary>
		/// Is this UOW transactional?
		/// Uses default value if not supplied.
		/// </summary>
		public bool? IsTransactional { get; set; }
		/// <summary>
		/// Timeout of UOW As milliseconds.
		/// Uses default value if not supplied.
		/// </summary>
		public TimeSpan? Timeout { get; set; }
		/// <summary>
		/// If this UOW is transactional, this option indicated the isolation level of the transaction.
		/// Uses default value if not supplied.
		/// </summary>
		public IsolationLevel? IsolationLevel { get; set; }
		/// <summary>
		/// This option should be set to <see cref="TransactionScopeAsyncFlowOption.Enabled"/>
		/// if unit of work is used in an async scope.
		/// </summary>
		public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }
		/// <summary>
		/// Can be used to enable/disable some filters. 
		/// </summary>
		public List<DataFilterConfiguration> FilterOverrides { get; } = new List<DataFilterConfiguration>();

		//internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
		//{
		//	//TODO: Do not change options object..?

		//	if(!IsTransactional.HasValue)
		//	{
		//		IsTransactional = defaultOptions.IsTransactional;
		//	}

		//	if(!Scope.HasValue)
		//	{
		//		Scope = defaultOptions.Scope;
		//	}

		//	if(!Timeout.HasValue && defaultOptions.Timeout.HasValue)
		//	{
		//		Timeout = defaultOptions.Timeout.Value;
		//	}

		//	if(!IsolationLevel.HasValue && defaultOptions.IsolationLevel.HasValue)
		//	{
		//		IsolationLevel = defaultOptions.IsolationLevel.Value;
		//	}
		//}

		//internal void FillOuterUowFiltersForNonProvidedOptions(List<DataFilterConfiguration> filterOverrides)
		//{
		//	foreach(var filterOverride in filterOverrides)
		//	{
		//		if(FilterOverrides.Any(fo => fo.FilterName == filterOverride.FilterName))
		//		{
		//			continue;
		//		}

		//		FilterOverrides.Add(filterOverride);
		//	}
		//}
	}
}

namespace System.Transactions
{
#if !NET46
	public enum TransactionScopeOption
	{
		Required,
		RequiresNew,
		Suppress,
	}

	/// <summary>Specifies the isolation level of a transaction.</summary>
	public enum IsolationLevel
	{
		Serializable,
		RepeatableRead,
		ReadCommitted,
		ReadUncommitted,
		Snapshot,
		Chaos,
		Unspecified,
	}

	/// <summary>[Supported in the .NET Framework 4.5.1 and later versions] Specifies whether transaction flow across thread continuations is enabled for <see cref="T:System.Transactions.TransactionScope" />.</summary>
	public enum TransactionScopeAsyncFlowOption
	{
		Suppress,
		Enabled,
	}
#endif
}