namespace LinFx.Extensions.Uow;

/// <summary>
/// 内部工作单元
/// </summary>
internal class ChildUnitOfWork : IUnitOfWork
{
    public Guid Id => _parent.Id;

    public IUnitOfWorkOptions Options => _parent.Options;

    public IUnitOfWork Outer => _parent.Outer;

    public bool IsReserved => _parent.IsReserved;

    public bool IsDisposed => _parent.IsDisposed;

    public bool IsCompleted => _parent.IsCompleted;

    public string ReservationName => _parent.ReservationName;

    public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
    public event EventHandler<UnitOfWorkEventArgs> Disposed;

    public IServiceProvider ServiceProvider => _parent.ServiceProvider;

    public Dictionary<string, object> Items => _parent.Items;

    private readonly IUnitOfWork _parent;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="parent">外部工作单元(带事务)</param>
    public ChildUnitOfWork(IUnitOfWork parent)
    {
        _parent = parent;
        _parent.Failed += (sender, args) => { Failed.InvokeSafely(sender, args); };
        _parent.Disposed += (sender, args) => { Disposed.InvokeSafely(sender, args); };
    }

    public void SetOuter(IUnitOfWork outer) => _parent.SetOuter(outer);

    public void Initialize(UnitOfWorkOptions options) => _parent.Initialize(options);

    public void Reserve(string reservationName) => _parent.Reserve(reservationName);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _parent.SaveChangesAsync(cancellationToken);

    public Task CompleteAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task RollbackAsync(CancellationToken cancellationToken = default) => _parent.RollbackAsync(cancellationToken);

    public void OnCompleted(Func<Task> handler) => _parent.OnCompleted(handler);

    public void AddOrReplaceLocalEvent(UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord>? replacementSelector = null) => _parent.AddOrReplaceLocalEvent(eventRecord, replacementSelector);

    public void AddOrReplaceDistributedEvent(UnitOfWorkEventRecord eventRecord, Predicate<UnitOfWorkEventRecord>? replacementSelector = null) => _parent.AddOrReplaceDistributedEvent(eventRecord, replacementSelector);

    public IDatabaseApi FindDatabaseApi(string key) => _parent.FindDatabaseApi(key);

    public void AddDatabaseApi(string key, IDatabaseApi api) => _parent.AddDatabaseApi(key, api);

    public IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory) => _parent.GetOrAddDatabaseApi(key, factory);

    public ITransactionApi FindTransactionApi(string key) => _parent.FindTransactionApi(key);

    public void AddTransactionApi(string key, ITransactionApi api) => _parent.AddTransactionApi(key, api);

    public ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory) => _parent.GetOrAddTransactionApi(key, factory);

    public void Dispose() { }

    public override string ToString() => $"[UnitOfWork {Id}]";
}