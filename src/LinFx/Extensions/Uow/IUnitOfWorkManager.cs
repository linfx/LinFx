using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元管理器
/// </summary>
public interface IUnitOfWorkManager
{
    /// <summary>
    /// 当前工作单元
    /// </summary>
    [AllowNull]
    IUnitOfWork Current { get; }

    IUnitOfWork Begin([NotNull] UnitOfWorkOptions options, bool requiresNew = false);

    IUnitOfWork Reserve([NotNull] string reservationName, bool requiresNew = false);

    void BeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkOptions options);

    bool TryBeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkOptions options);
}