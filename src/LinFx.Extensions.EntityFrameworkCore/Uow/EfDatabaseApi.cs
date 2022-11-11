using LinFx.Extensions.Uow;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

public class EfDatabaseApi : IDatabaseApi, ISupportsSavingChanges
{
    public IEfDbContext DbContext { get; }

    public EfDatabaseApi(IEfDbContext dbContext) => DbContext = dbContext;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => DbContext.SaveChangesAsync(cancellationToken);
}
