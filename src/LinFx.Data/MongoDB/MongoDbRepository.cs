using System.Threading.Tasks;
using MongoDB.Driver;
using LinFx.Domain.Entities;

namespace LinFx.Data.MongoDB
{
    public class MongoDbRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IMongoDatabaseProvider _provider;

        public MongoDbRepository(IMongoDatabaseProvider databaseProvider)
        {
            _provider = databaseProvider;
        }

        private IMongoCollection<TEntity> _collection
        {
            get { return _provider.Database.GetCollection<TEntity>(typeof(TEntity).Name); }
        }

        public Task InsertAsync(TEntity item)
        {
            return _collection.InsertOneAsync(item);
        }

        //public Task DeleteAsync(TEntity item)
        //{
        //    return _collection.DeleteOneAsync<TEntity>(item);
        //}

        //public Task UpdateAsync(TEntity item)
        //{
        //    return Collection.UpdateOneAsync(item.Id, item);
        //}
    }
}
