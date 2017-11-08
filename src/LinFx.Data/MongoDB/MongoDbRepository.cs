namespace LinFx.Data.MongoDB
{
    public class MongoDbRepository<TEntity>
    {
        private readonly IMongoDatabaseProvider _databaseProvider;

        public MongoDbRepository(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        //public virtual MongoCollection<TEntity> Collection
        //{
        //    get
        //    {
        //        return _databaseProvider.Database.GetCollection<TEntity>(typeof(TEntity).Name);
        //    }
        //}
    }
}
