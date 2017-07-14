//using LinFx.Data.Dapper.Extensions;
//using LinFx.Data.Dapper.Filters.Action;
//using LinFx.Data.Filters.Query;
//using LinFx.Domain.Entities;
//using System;
//using System.Data;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace LinFx.Data.Dapper.Repositories
//{
//    public class DapperRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
//        where TEntity : class, IEntity<TPrimaryKey>
//    {
//        public DapperRepository(IDbConnectionFactory factory) : base(factory)
//        {
//        }

//        public IDapperQueryFilterExecuter DapperQueryFilterExecuter { get; set; } = new DapperQueryFilterExecuter();

//        //public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

//        public IDapperActionFilterExecuter DapperActionFilterExecuter { get; set; }

//        public virtual IDbConnection Connection => _factory.Open();

//        public IDbTransaction ActiveTransaction = null;
//        //{
//        //    get { return Context.Database.CurrentTransaction.UnderlyingTransaction; }
//        //}

//        public int Count(Expression<Func<TEntity, bool>> predicate)
//        {
//            var filteredPredicate = DapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
//            return Connection.Count<TEntity>(filteredPredicate);
//        }

//        public override Task InsertAsync(TEntity item)
//        {
//            return Connection.InsertAsync(item);
//        }

//        public override Task UpdateAsync(TEntity item)
//        {
//            return Connection.UpdateAsync(item);
//        }

//        public override Task DeleteAsync(TEntity item)
//        {
//            throw new NotImplementedException();
//        }

//        public override Task DeleteAsync(TPrimaryKey id)
//        {
//            throw new NotImplementedException();
//        }

//        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
//        {
//            return Task.FromResult(FirstOrDefault((CreateEqualityExpressionForId(id))));
//        }

//        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
//        {
//            IPredicate pg = DapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
//            return Connection.GetList<TEntity>(pg, transaction: ActiveTransaction).FirstOrDefault();
//        }

//        public override Task<TEntity> GetAsync(TPrimaryKey id)
//        {
//            return FirstOrDefaultAsync(id);
//        }

//        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
//        {
//            ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity));

//            BinaryExpression lambdaBody = Expression.Equal(
//                Expression.PropertyOrField(lambdaParam, "Id"),
//                Expression.Constant(id, typeof(TPrimaryKey))
//            );

//            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
//        }
//    }

//    public class DapperRepository<TEntity> : DapperRepository<TEntity, string>, IRepository<TEntity>
//        where TEntity : class, IEntity<string>
//    {
//        public DapperRepository(IDbConnectionFactory factory) : base(factory)
//        {
//        }
//    }
//}
