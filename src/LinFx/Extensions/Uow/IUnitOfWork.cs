using JetBrains.Annotations;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元
/// </summary>
public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
{
    /// <summary>
    /// 唯一标识信息
    /// </summary>
    Guid Id { get; }

    Dictionary<string, object> Items { get; }

    //TODO: Switch to OnFailed (sync) and OnDisposed (sync) methods to be compatible with OnCompleted
    event EventHandler<UnitOfWorkFailedEventArgs> Failed;

    event EventHandler<UnitOfWorkEventArgs> Disposed;

    /// <summary>
    /// 配置信息
    /// </summary>
    IUnitOfWorkOptions Options { get; }

    IUnitOfWork Outer { get; }

    bool IsReserved { get; }

    bool IsDisposed { get; }

    bool IsCompleted { get; }

    string ReservationName { get; }

    void SetOuter([CanBeNull] IUnitOfWork outer);

    void Initialize([NotNull] UnitOfWorkOptions options);

    void Reserve([NotNull] string reservationName);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 提交工作单元
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    void OnCompleted(Func<Task> handler);

    /// <summary>
    /// 本地事件
    /// </summary>
    /// <param name="eventRecord"></param>
    /// <param name="replacementSelector"></param>
    void AddOrReplaceLocalEvent(UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord>? replacementSelector = null);

    /// <summary>
    /// 分布式事件
    /// </summary>
    /// <param name="eventRecord"></param>
    /// <param name="replacementSelector"></param>
    void AddOrReplaceDistributedEvent(UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord>? replacementSelector = null);
}
