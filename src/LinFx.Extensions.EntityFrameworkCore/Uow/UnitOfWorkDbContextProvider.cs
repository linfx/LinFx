using LinFx.Extensions.Data;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using LinFx.Extensions.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace LinFx.Extensions.EntityFrameworkCore.Uow;

/// <summary>
/// 工作单元数据库上下文提供者
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class UnitOfWorkDbContextProvider<TDbContext>(
    IUnitOfWorkManager unitOfWorkManager,
    IConnectionStringResolver connectionStringResolver,
    ICancellationTokenProvider cancellationTokenProvider,
    ICurrentTenant currentTenant,
    IOptions<EfDbContextOptions> options) : IDbContextProvider<TDbContext> where TDbContext : DbContext
{
    public ILogger<UnitOfWorkDbContextProvider<TDbContext>> Logger { get; set; } = NullLogger<UnitOfWorkDbContextProvider<TDbContext>>.Instance;

    private readonly IUnitOfWorkManager _unitOfWorkManager = unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver = connectionStringResolver;
    private readonly ICancellationTokenProvider _cancellationTokenProvider = cancellationTokenProvider;
    private readonly ICurrentTenant _currentTenant = currentTenant;
    private readonly EfDbContextOptions _options = options.Value;

    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<TDbContext> GetDbContextAsync()
    {
        // 获得当前的可用工作单元。
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
            throw new Exception("A DbContext can only be created inside a unit of work!");

        // 获得数据库连接上下文的连接字符串名称。
        // 根据名称解析具体的连接字符串。
        var targetDbContextType = _options.GetReplacedTypeOrSelf(typeof(TDbContext));
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        var connectionString = await ResolveConnectionStringAsync(connectionStringName);

        // 构造数据库上下文缓存 Key。
        // 从工作单元的缓存当中获取数据库上下文，不存在则调用 CreateDbContext() 创建。
        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
        if (databaseApi == null)
        {
            databaseApi = new EfDatabaseApi(await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString));
            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((EfDatabaseApi)databaseApi).DbContext;
    }

    /// <summary>
    /// 创建数据库上下文
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="connectionStringName"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
    {
        var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = await CreateDbContextAsync(unitOfWork);

            if (dbContext is EfDbContext efDbContext)
                efDbContext.Initialize(new EfDbContextInitializationContext(unitOfWork));

            return dbContext;
        }
    }

    /// <summary>
    /// 创建数据库上下文
    /// 如果是事务型的工作单元，则调用 CreateDbContextWithTransaction() 进行创建，
    /// 但不论如何都是通过工作单元提供的 IServiceProvider 解析出来 DbContext 的。
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <returns></returns>
    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork) => unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

    /// <summary>
    /// 创建数据库上下文
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <returns></returns>
    private async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";

        if (unitOfWork.FindTransactionApi(transactionApiKey) is not EfTransactionApi activeTransaction)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken())
                : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

            unitOfWork.AddTransactionApi(transactionApiKey, new EfTransactionApi(dbTransaction, dbContext, _cancellationTokenProvider));

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.As<DbContext>().HasRelationalTransactionManager())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                }
                else
                {
                    /* User did not re-use the ExistingConnection and we are starting a new transaction.
                     * EfCoreTransactionApi will check the connection string match and separately
                     * commit/rollback this transaction over the DbContext instance. */
                    if (unitOfWork.Options.IsolationLevel.HasValue)
                    {
                        await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken());
                    }
                    else
                    {
                        await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
                    }
                }
            }
            else
            {
                /* No need to store the returning IDbContextTransaction for non-relational databases
                 * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                 */
                await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);

            return dbContext;
        }
    }

    /// <summary>
    /// 根据名称解析具体的连接字符串
    /// </summary>
    /// <param name="connectionStringName"></param>
    /// <returns></returns>
    private async Task<string> ResolveConnectionStringAsync(string connectionStringName)
    {
        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (_currentTenant.Change(null))
            {
                return await _connectionStringResolver.ResolveAsync(connectionStringName);
            }
        }

        return await _connectionStringResolver.ResolveAsync(connectionStringName);
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default) => _cancellationTokenProvider.FallbackToProvider(preferredValue);
}
