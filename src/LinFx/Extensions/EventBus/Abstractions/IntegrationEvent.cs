using System;
using LinFx.Utils;

namespace LinFx.Extensions.EventBus
{
    public class IntegrationEvent
    {
        public long Id { get; }

        public long Timestamp { get; }

        public IntegrationEvent()
        {
            Id = IDUtils.NewId();
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
