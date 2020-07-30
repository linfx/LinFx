namespace LinFx.Extensions.MongoDB
{
    public interface IMongoModelSource
    {
        MongoDbContextModel GetModel(MongoDbContext dbContext);
    }
}
