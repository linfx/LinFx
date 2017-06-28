using LinFx.Data.Dapper.Extensions;
using System;
using System.Linq.Expressions;

namespace LinFx.Data.Dapper.Filters.Query
{
    public class NullDapperQueryFilterExecuter : IDapperQueryFilterExecuter
    {
        public static readonly NullDapperQueryFilterExecuter Instance = new NullDapperQueryFilterExecuter();

        public IPredicate ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public PredicateGroup ExecuteFilter<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
