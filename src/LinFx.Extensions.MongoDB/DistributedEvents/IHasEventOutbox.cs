using LinFx.Extensions.MongoDB;
using LinFx.Extensions.MongoDB.DistributedEvents;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB.DistributedEvents;

public interface IHasEventOutbox : IMongoDbContext
{
    //IMongoCollection<OutgoingEventRecord> OutgoingEvents { get; }
}
