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
/// ������Ԫ���ݿ��������ṩ��
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
    /// ��ȡ���ݿ�������
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<TDbContext> GetDbContextAsync()
    {
        // ��õ�ǰ�Ŀ��ù�����Ԫ��
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
            throw new Exception("A DbContext can only be created inside a unit of work!");

        // ������ݿ����������ĵ������ַ������ơ�
        // �������ƽ�������������ַ�����
        var targetDbContextType = _options.GetReplacedTypeOrSelf(typeof(TDbContext));
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        var connectionString = await ResolveConnectionStringAsync(connectionStringName);

        // �������ݿ������Ļ��� Key��
        // �ӹ�����Ԫ�Ļ��浱�л�ȡ���ݿ������ģ������������ CreateDbContext() ������
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
    /// �������ݿ�������
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
    /// �������ݿ�������
    /// ����������͵Ĺ�����Ԫ������� CreateDbContextWithTransaction() ���д�����
    /// ��������ζ���ͨ��������Ԫ�ṩ�� IServiceProvider �������� DbContext �ġ�
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <returns></returns>
    private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork) => unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

    /// <summary>
    /// �������ݿ�������
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
    /// �������ƽ�������������ַ���
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