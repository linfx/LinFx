using MongoDB.Driver;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinFx.Data.MongoDB
{
    public class MongoDbContext : IDisposable
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IMongoDatabase db)
        {
            _db = db;
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            string name;
            var type = typeof(T);
            var tableAttrs = type.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
            if (tableAttrs.Length > 0)
                name = tableAttrs[0].Name;
            else
                name = type.Name;

            return _db.GetCollection<T>(name);
        }

        public Task InsertAsync<T>(T item)
        {
            return GetCollection<T>().InsertOneAsync(item);
        }

        public async Task<long> DeleteAsync<T>(Expression<Func<T, bool>> filter)
        {
            try
            {
                var result = await GetCollection<T>().DeleteOneAsync(filter);
                return result.DeletedCount;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
        }
    }
}
