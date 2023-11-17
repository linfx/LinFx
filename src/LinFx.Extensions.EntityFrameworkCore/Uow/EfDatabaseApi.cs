using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

public class EfDatabaseApi : IDatabaseApi, ISupportsSavingChanges
{
    public DbContext DbContext { get; }

    public EfDatabaseApi(DbContext dbContext) => DbContext = dbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => DbContext.SaveChangesAsync(cancellationToken);
}
