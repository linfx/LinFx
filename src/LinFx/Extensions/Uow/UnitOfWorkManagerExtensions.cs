using JetBrains.Annotations;
using LinFx.Utils;
using System.Data;

namespace LinFx.Extensions.Uow;

public static class UnitOfWorkManagerExtensions
{
    [NotNull]
    public static IUnitOfWork Begin(
        [NotNull] this IUnitOfWorkManager unitOfWorkManager,
        bool requiresNew = false,
        bool isTransactional = false,
        IsolationLevel? isolationLevel = null,
        int? timeout = null)
    {
        Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));

        return unitOfWorkManager.Begin(new UnitOfWorkOptions
        {
            IsTransactional = isTransactional,
            IsolationLevel = isolationLevel,
            Timeout = timeout
        }, requiresNew);
    }

    public static void BeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName) => unitOfWorkManager.BeginReserved(reservationName, new UnitOfWorkOptions());

    public static void TryBeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName) => unitOfWorkManager.TryBeginReserved(reservationName, new UnitOfWorkOptions());
}
