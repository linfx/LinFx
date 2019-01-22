namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static async Task TimeoutAfterAsync(Func<CancellationToken, Task> action, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            using (var timeoutCts = new CancellationTokenSource(timeout))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
            {
                try
                {
                    await action(linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException exception)
                {
                    var timeoutReached = timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested;
                    if (timeoutReached)
                    {
                        throw new TimeoutException("Timeout", exception);
                    }
                    throw;
                }
            }
        }

        public static async Task<TResult> TimeoutAfterAsync<TResult>(Func<CancellationToken, Task<TResult>> action, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            using (var timeoutCts = new CancellationTokenSource(timeout))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
            {
                try
                {
                    return await action(linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException exception)
                {
                    var timeoutReached = timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested;
                    if (timeoutReached)
                    {
                        throw new TimeoutException("Timeout", exception);
                    }

                    throw;
                }
            }
        }
    }
}
