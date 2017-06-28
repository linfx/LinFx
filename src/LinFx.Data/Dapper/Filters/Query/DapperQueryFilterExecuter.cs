using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinFx.Data.Dapper.Extensions;
using LinFx.Data.Dapper.Expressions;

namespace LinFx.Data.Dapper.Filters.Query
{
    public class DapperQueryFilterExecuter : IDapperQueryFilterExecuter
    {
        private readonly IEnumerable<IDapperQueryFilter> _queryFilters = new List<IDapperQueryFilter>();

        public IPredicate ExecuteFilter<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            ICollection<IDapperQueryFilter> filters = _queryFilters.ToList();

            foreach (IDapperQueryFilter filter in filters)
            {
                predicate = filter.ExecuteFilter(predicate);
            }

            IPredicate pg = predicate.ToPredicateGroup();
            return pg;
        }

        public PredicateGroup ExecuteFilter<TEntity>() where TEntity : class
        {
            ICollection<IDapperQueryFilter> filters = _queryFilters.ToList();
            var groups = new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };

            foreach (IDapperQueryFilter filter in filters)
            {
                IFieldPredicate predicate = filter.ExecuteFilter<TEntity>();
                if (predicate != null)
                {
                    groups.Predicates.Add(predicate);
                }
            }

            return groups;
        }
    }
}
