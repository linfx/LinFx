using System;

namespace LinFx.Extensions.Auditing
{
    /// <summary>
    /// A standard interface to add CreationTime property.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time.
        /// </summary>
        DateTimeOffset CreationTime { get; set; }
    }
}