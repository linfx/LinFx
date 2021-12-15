using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static bool HasRelationalTransactionManager(this DbContext dbContext)
    {
        return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
    }

    public static bool MoveItem(this DbContext dbContext)
    {
        return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
    }
}
