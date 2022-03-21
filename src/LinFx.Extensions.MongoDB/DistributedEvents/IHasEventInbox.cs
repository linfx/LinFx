using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB.DistributedEvents;

public interface IHasEventInbox : IMongoDbContext
{
    //IMongoCollection<IncomingEventRecord> IncomingEvents { get; }
}
