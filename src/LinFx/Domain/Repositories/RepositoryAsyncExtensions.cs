using LinFx.Domain.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace LinFx.Domain.Repositories;

public static class RepositoryAsyncExtensions
{
    #region Contains

    public static async Task<bool> ContainsAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] T item,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.ContainsAsync(repository.Queryable, item, cancellationToken);

    #endregion

    #region Any/All

    public static async Task<bool> AnyAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AnyAsync(repository.Queryable, cancellationToken);

    public static async Task<bool> AnyAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AnyAsync(repository.Queryable, predicate, cancellationToken);

    public static async Task<bool> AllAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AllAsync(repository.Queryable, predicate, cancellationToken);

    #endregion

    #region Count/LongCount

    public static async Task<int> CountAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.CountAsync(repository.Queryable, cancellationToken);

    public static async Task<int> CountAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity => await repository.AsyncExecuter.CountAsync(repository.Queryable, predicate, cancellationToken);

    public static async Task<long> LongCountAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity => await repository.AsyncExecuter.LongCountAsync(repository.Queryable, cancellationToken);

    public static async Task<long> LongCountAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.LongCountAsync(repository.Queryable, predicate, cancellationToken);

    #endregion

    #region First/FirstOrDefault

    public static async Task<T> FirstAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity => await repository.AsyncExecuter.FirstAsync(repository.Queryable, cancellationToken);

    public static async Task<T> FirstAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.FirstAsync(repository.Queryable, predicate, cancellationToken);

    public static async Task<T> FirstOrDefaultAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.FirstOrDefaultAsync(repository.Queryable, cancellationToken);

    public static async Task<T> FirstOrDefaultAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.FirstOrDefaultAsync(repository.Queryable, predicate, cancellationToken);

    #endregion

    #region Last/LastOrDefault

    public static async Task<T> LastAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.LastAsync(repository.Queryable, cancellationToken);

    public static async Task<T> LastAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.LastAsync(repository.Queryable, predicate, cancellationToken);

    public static async Task<T> LastOrDefaultAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.LastOrDefaultAsync(repository.Queryable, cancellationToken);

    public static async Task<T> LastOrDefaultAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.LastOrDefaultAsync(repository.Queryable, predicate, cancellationToken);

    #endregion

    #region Single/SingleOrDefault

    public static async Task<T> SingleAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SingleAsync(repository.Queryable, cancellationToken);

    public static async Task<T> SingleAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SingleAsync(repository.Queryable, predicate, cancellationToken);

    public static async Task<T> SingleOrDefaultAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SingleOrDefaultAsync(repository.Queryable, cancellationToken);

    public static async Task<T> SingleOrDefaultAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SingleOrDefaultAsync(repository.Queryable, predicate, cancellationToken);

    #endregion

    #region Min

    public static async Task<T> MinAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.MinAsync(repository.Queryable, cancellationToken);

    public static async Task<TResult> MinAsync<T, TResult>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.MinAsync(repository.Queryable, selector, cancellationToken);

    #endregion

    #region Max

    public static async Task<T> MaxAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.MaxAsync(repository.Queryable, cancellationToken);

    public static async Task<TResult> MaxAsync<T, TResult>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.MaxAsync(repository.Queryable, selector, cancellationToken);

    #endregion

    #region Sum

    public static async Task<decimal> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, decimal>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<decimal?> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, decimal?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<int> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, int>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<int?> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, int?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<long> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, long>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<long?> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, long?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, double>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double?> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, double?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<float> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, float>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<float?> SumAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, float?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.SumAsync(repository.Queryable, selector, cancellationToken);

    #endregion

    #region Average

    public static async Task<decimal> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, decimal>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<decimal?> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, decimal?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, int>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double?> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, int?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, long>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double?> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, long?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, double>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<double?> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, double?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    public static async Task<float?> AverageAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        [NotNull] Expression<Func<T, float?>> selector,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.AverageAsync(repository.Queryable, selector, cancellationToken);

    #endregion

    #region ToList/Array

    public static async Task<List<T>> ToListAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity => await repository.AsyncExecuter.ToListAsync(repository.Queryable, cancellationToken);

    public static async Task<T[]> ToArrayAsync<T>(
        [NotNull] this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default)
        where T : class, IEntity
    => await repository.AsyncExecuter.ToArrayAsync(repository.Queryable, cancellationToken);

    #endregion
}
