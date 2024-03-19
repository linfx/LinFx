using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Uow;

[Service(ServiceLifetime.Singleton)]
[ExposeServices(typeof(IAmbientUnitOfWork), typeof(IUnitOfWorkAccessor))]
public class AmbientUnitOfWork : IAmbientUnitOfWork
{
    /// <inheritdoc/>
    public IUnitOfWork UnitOfWork => _currentUow.Value;

    private readonly AsyncLocal<IUnitOfWork> _currentUow;

    public AmbientUnitOfWork()
    {
        _currentUow = new AsyncLocal<IUnitOfWork>();
    }

    public void SetUnitOfWork(IUnitOfWork unitOfWork) => _currentUow.Value = unitOfWork;

    public IUnitOfWork GetCurrentByChecking()
    {
        var uow = UnitOfWork;

        //Skip reserved unit of work
        while (uow != null && (uow.IsReserved || uow.IsDisposed || uow.IsCompleted))
        {
            uow = uow.Outer;
        }

        return uow!;
    }
}