namespace LinFx.Extensions.MultiTenancy
{
    /// <summary>
    /// 多租户
    /// </summary>
    public interface IMultiTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        string TenantId { get; }
    }
}
