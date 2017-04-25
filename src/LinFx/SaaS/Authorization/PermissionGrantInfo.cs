namespace LinFx.SaaS.Authorization
{
    /// <summary>
    /// Represents a permission <see cref="Name"/> with <see cref="IsGranted"/> information.
    /// </summary>
    public class PermissionGrantInfo
    {
        /// <summary>
        /// Name of the permission.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Is this permission granted Prohibited?
        /// </summary>
        public bool IsGranted { get; private set; }
    }
}
