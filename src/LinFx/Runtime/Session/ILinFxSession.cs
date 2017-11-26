namespace LinFx.Session
{
    public interface ILinFxSession
    {
        /// <summary>
        /// Gets current UserId or null.
        /// It can be null if no user logged in.
        /// </summary>
        long UserId { get; }
        /// <summary>
        /// Gets current TenantId or null.
        /// This TenantId should be the TenantId of the <see cref="UserId"/>.
        /// It can be null if given <see cref="UserId"/> is a host user or no user logged in.
        /// </summary>
        int TenantId { get; }
    }
}
