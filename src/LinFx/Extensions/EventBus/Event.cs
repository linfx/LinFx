using LinFx.Timing;
using System;

namespace LinFx.Extensions.EventBus
{

    /// <summary>
    /// Implements <see cref="IEvent"/> and provides a base for event data classes.
    /// </summary>
    public class Event : IEvent
    {
        /// <summary>
        /// The time when the event occured.
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// The object which triggers the event (optional).
        /// </summary>
        public object EventSource { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Event() => EventTime = Clock.Now;
    }
}
