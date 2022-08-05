using LinFx.Extensions.Uow;

namespace LinFx.Extensions.EntityFrameworkCore;

public class EfDbContextInitializationContext
{
    public IUnitOfWork UnitOfWork { get; }

    public EfDbContextInitializationContext(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}
