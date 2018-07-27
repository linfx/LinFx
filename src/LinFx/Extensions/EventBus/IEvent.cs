using LinFx.Timing;
using System;

namespace LinFx.Extensions.EventBus
{
    /// <summary>
    /// Defines interface for all Event data classes.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// The time when the event occured.
        /// </summary>
        DateTime EventTime { get; set; }

        /// <summary>
        /// The object which triggers the event (optional).
        /// </summary>
        object EventSource { get; set; }
    }
}
