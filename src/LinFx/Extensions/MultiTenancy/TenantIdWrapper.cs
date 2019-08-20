namespace LinFx.Extensions.MultiTenancy
{
    public class TenantIdWrapper
    {
        /// <summary>
        /// Null indicates the host.
        /// Not null value for a tenant.
        /// </summary>
        public string TenantId { get; }

        public TenantIdWrapper(string tenantId)
        {
            TenantId = tenantId;
        }
    }
}
