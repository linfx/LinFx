using System;

namespace LinFx.Extensions.Mediator.Idempotency
{
    public class ClientRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }
    }
}
