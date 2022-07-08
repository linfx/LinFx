using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// 多租户
/// </summary>
public interface IMultiTenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Column("tenant_id"), StringLength(64)]
    string TenantId { get; }
}
