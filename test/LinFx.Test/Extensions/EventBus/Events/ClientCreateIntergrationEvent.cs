using LinFx.Extensions.EventBus;
using System.Collections.Generic;

namespace LinFx.Test.EventBus.Events
{
    public class ClientCreateIntergrationEvent : Event
    {
        public string ClientId { get; set; }
        public ICollection<string> AllowedScopes { get; set; }
        public ICollection<string> ClientSecrets { get; set; }
        public ICollection<string> AllowedGrantTypes { get; set; }
    }
}
