using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

public class EfDatabaseApi(DbContext context) : IDatabaseApi, ISupportsSavingChanges
{
    public DbContext DbContext { get; } = context;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => DbContext.SaveChangesAsync(cancellationToken);
}
