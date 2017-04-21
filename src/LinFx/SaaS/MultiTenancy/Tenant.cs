using LinFx.Domain.Entities;

namespace LinFx.SaaS.MultiTenancy
{
    /// <summary>
    /// 租户
    /// </summary>
    public class Tenant : Entity
    {
        public string Name { get; set; }
    }
}
