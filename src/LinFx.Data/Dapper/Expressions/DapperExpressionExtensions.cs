using LinFx.Data.Dapper.Extensions;
using System;
using System.Linq.Expressions;

namespace LinFx.Data.Dapper.Expressions
{
    internal static class DapperExpressionExtensions
    {
        public static IPredicate ToPredicateGroup<TEntity>(this Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DapperExpressionVisitor<TEntity>();
            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }
}
