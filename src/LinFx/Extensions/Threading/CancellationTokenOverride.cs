using System.Threading;

namespace LinFx.Extensions.Threading;

public class CancellationTokenOverride
{
    public CancellationToken CancellationToken { get; }

    public CancellationTokenOverride(CancellationToken cancellationToken)
    {
        CancellationToken = cancellationToken;
    }
}
