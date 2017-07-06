using System.Collections.Generic;

namespace LinFx.Domain.Uow
{
    /// <summary>
    /// Standard filters
    /// </summary>
    public static class DataFilters
    {
        /// <summary>
        /// "SoftDelete".
        /// Soft delete filter.
        /// Prevents getting deleted data from database.
        /// See <see cref="ISoftDelete"/> interface.
        /// </summary>
        public const string SoftDelete = "SoftDelete";
        /// <summary>
        /// "MustHaveTenant".
        /// Tenant filter to prevent getting data that is
        /// not belong to current tenant.
        /// </summary>
        public const string MustHaveTenant = "MustHaveTenant";
        /// <summary>
        /// "MayHaveTenant".
        /// Tenant filter to prevent getting data that is
        /// not belong to current tenant.
        /// </summary>
        public const string MayHaveTenant = "MayHaveTenant";
        /// <summary>
        /// Standard parameters of ABP.
        /// </summary>
        public static class Parameters
        {
            /// <summary>
            /// "TenantId".
            /// </summary>
            public const string TenantId = "TenantId";
        }
    }

	public class DataFilterConfiguration
	{
		public string FilterName { get; }

		public bool IsEnabled { get; }

		public IDictionary<string, object> FilterParameters { get; }

		public DataFilterConfiguration(string filterName, bool isEnabled)
		{
			FilterName = filterName;
			IsEnabled = isEnabled;
			FilterParameters = new Dictionary<string, object>();
		}

		internal DataFilterConfiguration(DataFilterConfiguration filterToClone, bool? isEnabled = null)
			: this(filterToClone.FilterName, isEnabled ?? filterToClone.IsEnabled)
		{
			foreach(var filterParameter in filterToClone.FilterParameters)
			{
				FilterParameters[filterParameter.Key] = filterParameter.Value;
			}
		}
	}
}
