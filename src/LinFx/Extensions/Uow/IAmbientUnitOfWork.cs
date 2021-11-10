namespace LinFx.Extensions.Uow
{
    public interface IAmbientUnitOfWork : IUnitOfWorkAccessor
    {
        IUnitOfWork GetCurrentByChecking();
    }
}