using LinFx.Extensions.EventBus.Events;
using System.Collections.Generic;

namespace LinFx.Test.Extensions.EventBus.Events
{
    public class ClientCreateIntergrationEvent : IntegrationEvent
    {
        public string ClientId { get; set; }
        public ICollection<string> AllowedScopes { get; set; }
        public ICollection<string> ClientSecrets { get; set; }
        public ICollection<string> AllowedGrantTypes { get; set; }
    }
}
