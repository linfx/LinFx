using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.Uow;

[Service(ServiceLifetime.Singleton)]
public class NullUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider
{
    public bool? IsTransactional => null;
}
