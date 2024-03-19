using LinFx.Extensions.Threading;
using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

/// <summary>
/// 数据库事务
/// </summary>
public class EfTransactionApi(
    IDbContextTransaction dbContextTransaction,
    DbContext starterDbContext,
    ICancellationTokenProvider cancellationTokenProvider) : ITransactionApi, ISupportsRollback
{
    public IDbContextTransaction DbContextTransaction { get; } = dbContextTransaction;

    public DbContext StarterDbContext { get; } = starterDbContext;

    public List<DbContext> AttendedDbContexts { get; } = new List<DbContext>();

    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;

    public async Task CommitAsync()
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.As<DbContext>().HasRelationalTransactionManager() && dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
                continue; //Relational databases use the shared transaction if they are using the same connection

            await dbContext.Database.CommitTransactionAsync(CancellationTokenProvider.Token);
        }

        await DbContextTransaction.CommitAsync(CancellationTokenProvider.Token);
    }

    public void Dispose() => DbContextTransaction.Dispose();

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.As<DbContext>().HasRelationalTransactionManager() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue; //Relational databases use the shared transaction if they are using the same connection
            }

            await dbContext.Database.RollbackTransactionAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        await DbContextTransaction.RollbackAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
    }
}
