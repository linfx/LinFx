using LinFx.Extensions.Threading;
using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

/// <summary>
/// 数据库事务
/// </summary>
public class EfTransactionApi : ITransactionApi, ISupportsRollback
{
    public IDbContextTransaction DbContextTransaction { get; }

    public DbContext StarterDbContext { get; }

    public List<DbContext> AttendedDbContexts { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public EfTransactionApi(
        IDbContextTransaction dbContextTransaction,
        DbContext starterDbContext,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        DbContextTransaction = dbContextTransaction;
        StarterDbContext = starterDbContext;
        CancellationTokenProvider = cancellationTokenProvider;
        AttendedDbContexts = new List<DbContext>();
    }

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
