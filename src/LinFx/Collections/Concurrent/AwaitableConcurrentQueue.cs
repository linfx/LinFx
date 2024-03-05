using System.Collections.Concurrent;

namespace LinFx.Collections.Concurrent;

public class AwaitableConcurrentQueue<T>
{
    private readonly ConcurrentQueue<T> msgs = new();
    private readonly ConcurrentQueue<TaskCompletionSource<T>> promises = new();

    public void Enqueue(T msg)
    {
        if (!promises.IsEmpty)
            if (promises.TryDequeue(out TaskCompletionSource<T> tcs))
                tcs.TrySetResult(msg);
        else
            msgs.Enqueue(msg);
    }

    public async Task<T> Dequeue()
    {
        var tcs = new TaskCompletionSource<T>();
        if (!msgs.IsEmpty)
            if (msgs.TryDequeue(out T msg))
                return msg;

        promises.Enqueue(tcs);
        return await tcs.Task;
    }
}