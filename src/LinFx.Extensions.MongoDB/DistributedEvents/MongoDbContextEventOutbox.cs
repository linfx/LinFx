//using LinFx.Extensions.Uow;
//using MongoDB.Driver;
//using MongoDB.Driver.Linq;
//using Volo.Abp.MongoDB.DistributedEvents;

//namespace LinFx.Extensions.MongoDB.DistributedEvents;

//public class MongoDbContextEventOutbox<TMongoDbContext> : IMongoDbContextEventOutbox<TMongoDbContext>
//    where TMongoDbContext : IHasEventOutbox
//{
//    protected IMongoDbContextProvider<TMongoDbContext> MongoDbContextProvider { get; }

//    public MongoDbContextEventOutbox(IMongoDbContextProvider<TMongoDbContext> mongoDbContextProvider)
//    {
//        MongoDbContextProvider = mongoDbContextProvider;
//    }

//    [UnitOfWork]
//    public virtual async Task EnqueueAsync(OutgoingEventInfo outgoingEvent)
//    {
//        var dbContext = (IHasEventOutbox)await MongoDbContextProvider.GetDbContextAsync();
//        if (dbContext.SessionHandle != null)
//        {
//            await dbContext.OutgoingEvents.InsertOneAsync(
//                dbContext.SessionHandle,
//                new OutgoingEventRecord(outgoingEvent)
//            );
//        }
//        else
//        {
//            await dbContext.OutgoingEvents.InsertOneAsync(
//                new OutgoingEventRecord(outgoingEvent)
//            );
//        }
//    }

//    [UnitOfWork]
//    public virtual async Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default)
//    {
//        var dbContext = (IHasEventOutbox)await MongoDbContextProvider.GetDbContextAsync(cancellationToken);

//        var outgoingEventRecords = await dbContext
//            .OutgoingEvents.AsQueryable()
//            .OrderBy(x => x.CreationTime)
//            .Take(maxCount)
//            .ToListAsync(cancellationToken: cancellationToken);

//        return outgoingEventRecords
//            .Select(x => x.ToOutgoingEventInfo())
//            .ToList();
//    }

//    [UnitOfWork]
//    public virtual async Task DeleteAsync(Guid id)
//    {
//        var dbContext = (IHasEventOutbox)await MongoDbContextProvider.GetDbContextAsync();
//        if (dbContext.SessionHandle != null)
//        {
//            await dbContext.OutgoingEvents.DeleteOneAsync(dbContext.SessionHandle, x => x.Id.Equals(id));
//        }
//        else
//        {
//            await dbContext.OutgoingEvents.DeleteOneAsync(x => x.Id.Equals(id));
//        }
//    }
//}
