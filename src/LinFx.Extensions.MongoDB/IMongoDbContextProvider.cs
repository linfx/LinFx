namespace LinFx.Extensions.MongoDB;

public interface IMongoDbContextProvider<TMongoDbContext>
    where TMongoDbContext : IMongoDbContext
{
    Task<TMongoDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}
