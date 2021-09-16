using LinFx.Extensions.Uow;

namespace LinFx.EntityFrameworkCore
{
    public class EfCoreDbContextInitializationContext
    {
        public IUnitOfWork UnitOfWork { get; }

        public EfCoreDbContextInitializationContext(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}