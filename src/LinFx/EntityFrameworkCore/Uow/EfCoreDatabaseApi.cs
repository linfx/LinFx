using LinFx.Extensions.Uow;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.EntityFrameworkCore.Uow
{
    public class EfCoreDatabaseApi : IDatabaseApi, ISupportsSavingChanges
    {
        public IEfCoreDbContext DbContext { get; }

        public EfCoreDatabaseApi(IEfCoreDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}