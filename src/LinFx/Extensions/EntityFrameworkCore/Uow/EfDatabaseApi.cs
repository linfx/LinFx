using LinFx.Extensions.Uow;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

public class EfDatabaseApi : IDatabaseApi, ISupportsSavingChanges
{
    public IEfDbContext DbContext { get; }

    public EfDatabaseApi(IEfDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
