namespace LinFx.Extensions.Uow
{
    public interface IUnitOfWorkTransactionBehaviourProvider
    {
        bool? IsTransactional { get; }
    }
}