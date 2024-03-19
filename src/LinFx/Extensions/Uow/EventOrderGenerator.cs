namespace LinFx.Extensions.Uow;

public static class EventOrderGenerator
{
    private static long _lastOrder;

    public static long GetNext() => Interlocked.Increment(ref _lastOrder);
}