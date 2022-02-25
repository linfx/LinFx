using LinFx.Extensions.Threading;
using MongoDB.Driver;

namespace LinFx.Extensions.Uow.MongoDB;

public class MongoDbTransactionApi : ITransactionApi, ISupportsRollback
{
    public IClientSessionHandle SessionHandle { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public MongoDbTransactionApi(
        IClientSessionHandle sessionHandle,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        SessionHandle = sessionHandle;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public async Task CommitAsync()
    {
        await SessionHandle.CommitTransactionAsync(CancellationTokenProvider.Token);
    }

    public void Dispose()
    {
        SessionHandle.Dispose();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await SessionHandle.AbortTransactionAsync(
            CancellationTokenProvider.FallbackToProvider(cancellationToken)
        );
    }
}
