//using LinFx.Domain.Entities;
//using MongoDB.Driver;
//using System.Threading.Tasks;

//namespace LinFx.Data.MongoDB
//{
//    public class MongoDbRepository<TEntity>
//        where TEntity : class, IEntity
//    {
//        private readonly IMongoDatabaseProvider _provider;

//        public MongoDbRepository(IMongoDatabaseProvider databaseProvider)
//        {
//            _provider = databaseProvider;
//        }

//        public virtual IMongoCollection<TEntity> Collection
//        {
//            get { return _provider.Database.GetCollection<TEntity>(typeof(TEntity).Name); }
//        }

//        public Task InsertAsync(TEntity item)
//        {
//            return Collection.InsertOneAsync(item);
//        }

//        public Task DeleteAsync(TEntity item)
//        {
//            return Collection.DeleteOneAsync(item.Id);
//        }



//        //public Task UpdateAsync(TEntity item)
//        //{
//        //    return Collection.UpdateOneAsync(item.Id, item);
//        //}
//    }
//}
