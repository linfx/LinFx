using JetBrains.Annotations;
using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace LinFx.Extensions.Uow;

/// <summary>
/// 工作单元
/// </summary>
public class UnitOfWork(IServiceProvider serviceProvider, IUnitOfWorkEventPublisher unitOfWorkEventPublisher, IOptions<UnitOfWorkDefaultOptions> options) : IUnitOfWork, ITransientDependency
{
    /// <summary>
    /// Default: false.
    /// </summary>
    public static bool EnableObsoleteDbContextCreationWarning { get; } = false;

    /// <summary>
    /// Name
    /// </summary>
    public const string UnitOfWorkReservationName = "_ActionUnitOfWork";

    public Guid Id { get; } = Guid.NewGuid();

    public IUnitOfWorkOptions Options { get; private set; }

    /// <summary>
    /// 外部工作单元
    /// </summary>
    public IUnitOfWork Outer { get; private set; }

    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// 领域事件发布器
    /// </summary>
    protected IUnitOfWorkEventPublisher UnitOfWorkEventPublisher { get; } = unitOfWorkEventPublisher;

    /// <summary>
    /// 是否保留
    /// </summary>
    public bool IsReserved { get; set; }

    public bool IsDisposed { get; private set; }

    public bool IsCompleted { get; private set; }

    public string ReservationName { get; set; }

    public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
    public event EventHandler<UnitOfWorkEventArgs> Disposed;

    protected List<Func<Task>> CompletedHandlers { get; } = [];

    /// <summary>
    /// 本地事件
    /// </summary>
    protected List<UnitOfWorkEventRecord> LocalEvents { get; } = [];

    /// <summary>
    /// 分步式事件
    /// </summary>
    protected List<UnitOfWorkEventRecord> DistributedEvents { get; } = [];

    [NotNull]
    public Dictionary<string, object> Items { get; } = [];

    private readonly Dictionary<string, IDatabaseApi> _databaseApis = [];
    private readonly Dictionary<string, ITransactionApi> _transactionApis = [];
    private readonly UnitOfWorkDefaultOptions _defaultOptions = options.Value;

    private Exception _exception;
    private bool _isCompleting;
    private bool _isRolledback;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="Exception"></exception>
    public virtual void Initialize(UnitOfWorkOptions options)
    {
        if (Options != null)
            throw new Exception("This unit of work is already initialized before!");

        Options = _defaultOptions.Normalize(options.Clone());
        IsReserved = false;
    }

    public virtual void Reserve(string reservationName)
    {
        Check.NotNull(reservationName, nameof(reservationName));

        ReservationName = reservationName;
        IsReserved = true;
    }

    /// <summary>
    /// 设置外部工作单元
    /// </summary>
    /// <param name="outer"></param>
    public virtual void SetOuter(IUnitOfWork outer) => Outer = outer;

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback)
            return;

        // 遍历集合，如果对象实现了 ISupportsSavingChanges 则调用相应的方法进行数据持久化
        foreach (var databaseApi in GetAllActiveDatabaseApis())
        {
            if (databaseApi is ISupportsSavingChanges api)
                await api.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual IReadOnlyList<IDatabaseApi> GetAllActiveDatabaseApis() => _databaseApis.Values.ToImmutableList();

    public virtual IReadOnlyList<ITransactionApi> GetAllActiveTransactionApis() => _transactionApis.Values.ToImmutableList();

    public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        // 是否已经进行了回滚操作，如果进行了回滚操作，则不提交工作单元
        if (_isRolledback)
            return;

        // 防止多次调用 Complete 方法，原理就是看 _isCompleting 或者 IsCompleted 是不是已经为 True 了
        PreventMultipleComplete();

        try
        {
            _isCompleting = true;
            await SaveChangesAsync(cancellationToken);

            while (LocalEvents.Any() || DistributedEvents.Any())
            {
                // 发布本地事件
                if (LocalEvents.Any())
                {
                    var localEventsToBePublished = LocalEvents.OrderBy(e => e.EventOrder).ToArray();
                    LocalEvents.Clear();
                    await UnitOfWorkEventPublisher.PublishLocalEventsAsync(localEventsToBePublished);
                }

                // 发布分布式事件
                if (DistributedEvents.Any())
                {
                    var distributedEventsToBePublished = DistributedEvents.OrderBy(e => e.EventOrder).ToArray();
                    DistributedEvents.Clear();
                    await UnitOfWorkEventPublisher.PublishDistributedEventsAsync(distributedEventsToBePublished);
                }

                await SaveChangesAsync(cancellationToken);
            }

            await CommitTransactionsAsync();
            IsCompleted = true;

            // 数据储存了，事务提交了，则说明工作单元已经完成了，遍历完成事件集合，依次调用这些方法
            await OnCompletedAsync();
        }
        catch (Exception ex)
        {
            // 一旦在持久化或者是提交事务时出现了异常，则往上层抛出
            _exception = ex;
            throw;
        }
    }

    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback)
            return;

        _isRolledback = true;

        await RollbackAllAsync(cancellationToken);
    }

    public virtual IDatabaseApi FindDatabaseApi(string key) => _databaseApis.GetOrDefault(key);

    public virtual void AddDatabaseApi(string key, IDatabaseApi api)
    {
        Check.NotNull(key, nameof(key));
        Check.NotNull(api, nameof(api));

        if (_databaseApis.ContainsKey(key))
            throw new Exception("There is already a database API in this unit of work with given key: " + key);

        _databaseApis.Add(key, api);
    }

    public virtual IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory) => _databaseApis.GetOrAdd(key, factory);

    public virtual ITransactionApi FindTransactionApi(string key) => _transactionApis.GetOrDefault(key);

    public virtual void AddTransactionApi(string key, ITransactionApi api)
    {
        Check.NotNull(key, nameof(key));
        Check.NotNull(api, nameof(api));

        if (_transactionApis.ContainsKey(key))
            throw new Exception("There is already a transaction API in this unit of work with given key: " + key);

        _transactionApis.Add(key, api);
    }

    public virtual ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory) => _transactionApis.GetOrAdd(key, factory);

    public virtual void OnCompleted(Func<Task> handler) => CompletedHandlers.Add(handler);

    public virtual void AddOrReplaceLocalEvent(UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord> replacementSelector = null) => AddOrReplaceEvent(LocalEvents, eventRecord, replacementSelector);

    public virtual void AddOrReplaceDistributedEvent(UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord> replacementSelector = null) => AddOrReplaceEvent(DistributedEvents, eventRecord, replacementSelector);

    public virtual void AddOrReplaceEvent(List<UnitOfWorkEventRecord> eventRecords, UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord> replacementSelector = null)
    {
        if (replacementSelector == null)
        {
            eventRecords.Add(eventRecord);
        }
        else
        {
            var foundIndex = eventRecords.FindIndex(replacementSelector);
            if (foundIndex < 0)
                eventRecords.Add(eventRecord);
            else
                eventRecords[foundIndex] = eventRecord;
        }
    }

    protected virtual async Task OnCompletedAsync()
    {
        foreach (var handler in CompletedHandlers)
        {
            await handler.Invoke();
        }
    }

    protected virtual void OnFailed() => Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(this, _exception, _isRolledback));

    protected virtual void OnDisposed() => Disposed.InvokeSafely(this, new UnitOfWorkEventArgs(this));

    public virtual void Dispose()
    {
        if (IsDisposed)
            return;

        IsDisposed = true;

        DisposeTransactions();

        if (!IsCompleted || _exception != null)
            OnFailed();

        OnDisposed();
    }

    private void DisposeTransactions()
    {
        foreach (var transactionApi in GetAllActiveTransactionApis())
        {
            try
            {
                transactionApi.Dispose();
            }
            catch { }
        }
    }

    private void PreventMultipleComplete()
    {
        if (IsCompleted || _isCompleting)
            throw new Exception("Complete is called before!");
    }

    protected virtual async Task RollbackAllAsync(CancellationToken cancellationToken)
    {
        foreach (var databaseApi in GetAllActiveDatabaseApis())
        {
            if (databaseApi is ISupportsRollback api)
            {
                try
                {
                    await api.RollbackAsync(cancellationToken);
                }
                catch { }
            }
        }

        foreach (var transactionApi in GetAllActiveTransactionApis())
        {
            if (transactionApi is ISupportsRollback api)
            {
                try
                {
                    await api.RollbackAsync(cancellationToken);
                }
                catch { }
            }
        }
    }

    protected virtual async Task CommitTransactionsAsync()
    {
        foreach (var transaction in GetAllActiveTransactionApis())
        {
            await transaction.CommitAsync();
        }
    }

    public override string ToString() => $"[UnitOfWork {Id}]";
}
