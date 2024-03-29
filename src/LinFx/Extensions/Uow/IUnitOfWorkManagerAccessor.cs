namespace LinFx.Extensions.Uow;

public interface IUnitOfWorkManagerAccessor
{
    IUnitOfWorkManager UnitOfWorkManager { get; }
}
