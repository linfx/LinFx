using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.MongoDB.MongoDB;
using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB;

public abstract class MongoDbContext : IMongoDbContext, ITransientDependency
{
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    public IMongoModelSource ModelSource { get; set; }

    public IMongoClient Client { get; private set; }

    public IMongoDatabase Database { get; private set; }

    public IClientSessionHandle SessionHandle { get; private set; }

    protected internal virtual void CreateModel(IMongoModelBuilder modelBuilder)
    {
    }

    public virtual void InitializeDatabase(IMongoDatabase database, IMongoClient client, IClientSessionHandle sessionHandle)
    {
        Database = database;
        Client = client;
        SessionHandle = sessionHandle;
    }

    public virtual IMongoCollection<T> Collection<T>()
    {
        return Database.GetCollection<T>(GetCollectionName<T>());
    }

    public virtual void InitializeCollections(IMongoDatabase database)
    {
        Database = database;
        ModelSource.GetModel(this);
    }

    protected virtual string GetCollectionName<T>()
    {
        return GetEntityModel<T>().CollectionName;
    }

    protected virtual IMongoEntityModel GetEntityModel<TEntity>()
    {
        //var model = ModelSource.GetModel(this).Entities.GetOrDefault(typeof(TEntity));

        //if (model == null)
        //    throw new Exception("Could not find a model for given entity type: " + typeof(TEntity).AssemblyQualifiedName);

        //return model;

        throw new NotImplementedException();
    }
}
