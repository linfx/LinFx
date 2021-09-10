using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace LinFx.Extensions.Uow
{
    [ExposeServices(typeof(IAmbientUnitOfWork), typeof(IUnitOfWorkAccessor))]
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class AmbientUnitOfWork : IAmbientUnitOfWork
    {
        public IUnitOfWork UnitOfWork => _currentUow.Value;

        private readonly AsyncLocal<IUnitOfWork> _currentUow;

        public AmbientUnitOfWork()
        {
            _currentUow = new AsyncLocal<IUnitOfWork>();
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }

        public IUnitOfWork GetCurrentByChecking()
        {
            var uow = UnitOfWork;

            //Skip reserved unit of work
            while (uow != null && (uow.IsReserved || uow.IsDisposed || uow.IsCompleted))
            {
                uow = uow.Outer;
            }

            return uow;
        }
    }
}