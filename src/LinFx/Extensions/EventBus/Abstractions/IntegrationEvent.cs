using System;
using LinFx.Utils;

namespace LinFx.Extensions.EventBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = IDUtils.CreateNewId();
            Timestamp = DateTime.UtcNow.ToUnixTimeSeconds();
        }

        public long Id { get; }

        public long Timestamp { get; }
    }
}
