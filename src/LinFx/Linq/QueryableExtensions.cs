using JetBrains.Annotations;
using LinFx.Utils;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq;

/// <summary>
/// Some useful extension methods for <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool isDescending = false)
    {
        if (source == null)
            throw new ArgumentException(nameof(source));

        if (string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentException(nameof(propertyName));

        Type type = typeof(T);
        ParameterExpression arg = Expression.Parameter(type, "x");
        PropertyInfo propertyInfo = type.GetProperty(propertyName);
        Expression expression = Expression.Property(arg, propertyInfo);
        type = propertyInfo.PropertyType;

        Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        LambdaExpression lambda = Expression.Lambda(delegateType, expression, arg);

        var methodName = isDescending ? "OrderByDescending" : "OrderBy";
        object result = typeof(Queryable).GetMethods().Single(
            method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, new object[] { source, lambda });
        return (IQueryable<T>)result;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? query.Where(predicate)
            : query;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Where(predicate)
            : query;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? query.Where(predicate)
            : query;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Where(predicate)
            : query;
    }
}
