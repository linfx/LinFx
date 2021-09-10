using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Uow
{
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class NullUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider
    {
        public bool? IsTransactional => null;
    }
}