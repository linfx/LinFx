using LinFx.Extensions.Dapper;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinFx.Extensions.Data.Expressions
{
    internal static class DataAccessExpressionExtensions
    {
        public static IPredicate ToPredicateGroup<TEntity>(this Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DataAccessExpressionVisitor<TEntity>();
            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }

    public class PredicateBuilder<T>
    {
        Expression<Func<T, bool>> _expr;

        public static PredicateBuilder<T> Where()
        {
            return new PredicateBuilder<T>();
        }

        public PredicateBuilder<T> And(Expression<Func<T, bool>> expr)
        {
            if (_expr == null)
            {
                _expr = expr;
                return this;
            }
            _expr = _expr.And(expr);
            return this;
        }

        public Expression<Func<T, bool>> Predicate
        {
            get { return _expr; }
        }
    }

    public static class PredicateBuilderExtensions
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
