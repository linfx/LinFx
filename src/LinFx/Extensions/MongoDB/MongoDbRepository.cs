using System.Threading.Tasks;
using MongoDB.Driver;
using LinFx.Domain.Models;

namespace LinFx.Extensions.MongoDB
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
        //    return _collection.DeleteOneAsync(item);
        //}

        //public Task UpdateAsync(TEntity item)
        //{
        //    return Collection.UpdateOneAsync(item.Id, item);
        //}
    }
}
