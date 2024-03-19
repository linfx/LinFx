namespace LinFx;

/// <summary>
/// This class can be used to provide an action when
/// Dipose method is called.
/// </summary>
/// <remarks>
/// Creates a new <see cref="DisposeAction"/> object.
/// </remarks>
/// <param name="action">Action to be executed when this object is disposed.</param>
public class DisposeAction(Action? action) : IDisposable
{
    private Action? _action = action;
    public static readonly DisposeAction Empty = new(default);

    public void Dispose()
    {
        // Interlocked prevents multiple execution of the _action.
        var action = Interlocked.Exchange(ref _action, null);
        action?.Invoke();
    }
}
