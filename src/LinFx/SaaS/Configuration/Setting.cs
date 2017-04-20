using LinFx.Domain.Entities;

namespace LinFx.Zero.Configuration
{
    public class Setting : Entity
    {
        /// <summary>
        /// TenantId for this setting.
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of the setting.
        /// </summary>
        public string Value { get; set; }
    }

    public class OrganizationUnitSettings
    {
    }
}
