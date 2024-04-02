using System.Diagnostics.CodeAnalysis;
using System.Data;

namespace LinFx.Extensions.Uow;

public static class UnitOfWorkManagerExtensions
{
    public static IUnitOfWork Begin(this IUnitOfWorkManager unitOfWorkManager, bool requiresNew = false, bool isTransactional = false, IsolationLevel? isolationLevel = null, int? timeout = null) => unitOfWorkManager.Begin(new UnitOfWorkOptions
    {
        IsTransactional = isTransactional,
        IsolationLevel = isolationLevel,
        Timeout = timeout
    }, requiresNew);

    public static void BeginReserved(this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName) => unitOfWorkManager.BeginReserved(reservationName, new UnitOfWorkOptions());

    public static void TryBeginReserved(this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName) => unitOfWorkManager.TryBeginReserved(reservationName, new UnitOfWorkOptions());
}
