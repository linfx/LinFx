using LinFx.Utils;
using System;

namespace LinFx.Extensions.EventBus
{
    [Obsolete]
    public class Event : IEvent
    {
        public long Id { get; }

        public long Timestamp { get; }

        public Event()
        {
            Id = IDUtils.NewId();
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
