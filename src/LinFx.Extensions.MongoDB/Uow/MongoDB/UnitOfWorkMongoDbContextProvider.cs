using LinFx.Extensions.Data;
using LinFx.Extensions.MongoDB;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LinFx.Extensions.Uow.MongoDB;

public class UnitOfWorkMongoDbContextProvider<TMongoDbContext> : IMongoDbContextProvider<TMongoDbContext>
    where TMongoDbContext : IMongoDbContext
{
    private const string TransactionsNotSupportedErrorMessage = "Current database does not support transactions. Your database may remain in an inconsistent state in an error case.";
    public ILogger<UnitOfWorkMongoDbContextProvider<TMongoDbContext>> Logger { get; set; }

    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly ICurrentTenant _currentTenant;
    private readonly MongoDbContextOptions _options;

    public UnitOfWorkMongoDbContextProvider(
        IUnitOfWorkManager unitOfWorkManager,
        IConnectionStringResolver connectionStringResolver,
        ICancellationTokenProvider cancellationTokenProvider,
        ICurrentTenant currentTenant,
        IOptions<MongoDbContextOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _connectionStringResolver = connectionStringResolver;
        _cancellationTokenProvider = cancellationTokenProvider;
        _currentTenant = currentTenant;
        _options = options.Value;

        Logger = NullLogger<UnitOfWorkMongoDbContextProvider<TMongoDbContext>>.Instance;
    }

    public async Task<TMongoDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new Exception(
                $"A {nameof(IMongoDatabase)} instance can only be created inside a unit of work!");
        }

        var targetDbContextType = _options.GetReplacedTypeOrSelf(typeof(TMongoDbContext));
        var connectionString = await ResolveConnectionStringAsync(targetDbContextType);
        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var mongoUrl = new MongoUrl(connectionString);
        var databaseName = mongoUrl.DatabaseName;
        if (databaseName.IsNullOrWhiteSpace())
        {
            databaseName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        }

        //TODO: Create only single MongoDbClient per connection string in an application (extract MongoClientCache for example).
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
        if (databaseApi == null)
        {
            databaseApi = new MongoDbDatabaseApi(
                await CreateDbContextAsync(
                    unitOfWork,
                    mongoUrl,
                    databaseName,
                    cancellationToken
                )
            );

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TMongoDbContext)((MongoDbDatabaseApi)databaseApi).DbContext;
    }

    private async Task<TMongoDbContext> CreateDbContextAsync(
        IUnitOfWork unitOfWork,
        MongoUrl mongoUrl,
        string databaseName,
        CancellationToken cancellationToken = default)
    {
        var client = CreateMongoClient(mongoUrl);
        var database = client.GetDatabase(databaseName);

        if (unitOfWork.Options.IsTransactional)
        {
            return await CreateDbContextWithTransactionAsync(
                unitOfWork,
                mongoUrl,
                client,
                database,
                cancellationToken
            );
        }

        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();
        dbContext.ToMongoDbContext().InitializeDatabase(database, client, null);

        return dbContext;
    }

    private async Task<TMongoDbContext> CreateDbContextWithTransactionAsync(
        IUnitOfWork unitOfWork,
        MongoUrl url,
        MongoClient client,
        IMongoDatabase database,
        CancellationToken cancellationToken = default)
    {
        var transactionApiKey = $"MongoDb_{url}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as MongoDbTransactionApi;
        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();

        if (activeTransaction?.SessionHandle == null)
        {
            var session = await client.StartSessionAsync(cancellationToken: GetCancellationToken(cancellationToken));

            if (unitOfWork.Options.Timeout.HasValue)
            {
                session.AdvanceOperationTime(new BsonTimestamp(unitOfWork.Options.Timeout.Value));
            }

            try
            {
                session.StartTransaction();
            }
            catch (NotSupportedException e)
            {
                Logger.LogError(TransactionsNotSupportedErrorMessage);
                Logger.LogException(e);

                dbContext.ToMongoDbContext().InitializeDatabase(database, client, null);
                return dbContext;
            }

            unitOfWork.AddTransactionApi(
                transactionApiKey,
                new MongoDbTransactionApi(
                    session,
                    _cancellationTokenProvider
                )
            );

            dbContext.ToMongoDbContext().InitializeDatabase(database, client, session);
        }
        else
        {
            dbContext.ToMongoDbContext().InitializeDatabase(database, client, activeTransaction.SessionHandle);
        }

        return dbContext;
    }

    private async Task<string> ResolveConnectionStringAsync(Type dbContextType)
    {
        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TMongoDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (_currentTenant.Change(null))
            {
                return await _connectionStringResolver.ResolveAsync(dbContextType);
            }
        }

        return await _connectionStringResolver.ResolveAsync(dbContextType);
    }

    private MongoClient CreateMongoClient(MongoUrl mongoUrl)
    {
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
        _options.MongoClientSettingsConfigurer?.Invoke(mongoClientSettings);

        return new MongoClient(mongoClientSettings);
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return _cancellationTokenProvider.FallbackToProvider(preferredValue);
    }
}
