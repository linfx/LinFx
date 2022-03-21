namespace LinFx.Extensions.MongoDB;

public static class MongoDbContextExtensions
{
    public static MongoDbContext ToMongoDbContext(this IMongoDbContext dbContext)
    {
        var abpMongoDbContext = dbContext as MongoDbContext;

        if (abpMongoDbContext == null)
            throw new Exception($"The type {dbContext.GetType().AssemblyQualifiedName} should be convertable to {typeof(MongoDbContext).AssemblyQualifiedName}!");

        return abpMongoDbContext;
    }
}
