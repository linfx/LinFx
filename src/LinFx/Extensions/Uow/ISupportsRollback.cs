using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow
{
    public interface ISupportsRollback
    {
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
