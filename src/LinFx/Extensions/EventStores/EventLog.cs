using LinFx.Extensions.EventBus;
using LinFx.Utils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LinFx.Extensions.EventStores
{
    public class EventLog
    {
        private EventLog() { }

        public EventLog(IntegrationEvent evt)
        {
            EventId = evt.Id;
            CreationTime = DateTime.Now;
            EventTypeName = evt.GetType().FullName;
            Content = evt.ToJsonString();
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }

        public long EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }

        public EventLog DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
            return this;
        }
    }
}