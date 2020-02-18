using System.Collections.Generic;
using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoModelSource ModelSource = new MongoModelSource();

        public IMongoDatabase Database { get; private set; }

        protected internal virtual void CreateModel(IMongoModelBuilder modelBuilder)
        {
        }

        public virtual void InitializeDatabase(IMongoDatabase database)
        {
            Database = database;
        }

        public virtual IMongoCollection<T> Collection<T>()
        {
            return Database.GetCollection<T>(GetCollectionName<T>());
        }

        protected virtual string GetCollectionName<T>()
        {
            return GetEntityModel<T>().CollectionName;
        }

        protected virtual IMongoEntityModel GetEntityModel<TEntity>()
        {
            var model = ModelSource.GetModel(this).Entities.GetOrDefault(typeof(TEntity));

            if (model == null)
                throw new LinFxException("Could not find a model for given entity type: " + typeof(TEntity).AssemblyQualifiedName);

            return model;
        }

    }
}