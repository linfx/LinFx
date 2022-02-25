using LinFx.Extensions.MongoDB;

namespace LinFx.Extensions.Uow.MongoDB;

public class MongoDbDatabaseApi : IDatabaseApi
{
    public IMongoDbContext DbContext { get; }

    public MongoDbDatabaseApi(IMongoDbContext dbContext)
    {
        DbContext = dbContext;
    }
}
