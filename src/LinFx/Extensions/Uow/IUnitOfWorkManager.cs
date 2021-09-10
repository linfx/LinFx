using JetBrains.Annotations;

namespace LinFx.Extensions.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IUnitOfWork Begin([NotNull] UnitOfWorkOptions options, bool requiresNew = false);

        [NotNull]
        IUnitOfWork Reserve([NotNull] string reservationName, bool requiresNew = false);

        void BeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkOptions options);

        bool TryBeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkOptions options);
    }
}