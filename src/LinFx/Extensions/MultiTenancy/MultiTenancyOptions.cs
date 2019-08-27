namespace LinFx.Extensions.MultiTenancy
{
    public class MultiTenancyOptions
    {
        /// <summary>
        /// A central point to enable/disable multi-tenancy.
        /// Default: false. 
        /// </summary>
        public bool IsEnabled { get; set; }

        public string TenantKey { get; set; } = "__tenant";
    }
}