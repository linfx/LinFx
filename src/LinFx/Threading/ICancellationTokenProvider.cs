using System;
using System.Threading;

namespace LinFx.Threading
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }

        IDisposable Use(CancellationToken cancellationToken);
    }
}
