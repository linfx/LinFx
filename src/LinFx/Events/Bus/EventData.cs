using LinFx.Timing;
using System;

namespace LinFx.Events.Bus
{
    /// <summary>
    /// Defines interface for all Event data classes.
    /// </summary>
    public interface IEventData
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

    /// <summary>
    /// Implements <see cref="IEventData"/> and provides a base for event data classes.
    /// </summary>
    public abstract class EventData : IEventData
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
        protected EventData()
        {
            EventTime = Clock.Now;
        }
    }
}
