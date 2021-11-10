using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow
{
    public interface ISupportsSavingChanges
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}