namespace LinFx.Extensions.EventBus;

/// <summary>
/// This <see cref="IEventHandlerFactory"/> implementation is used to handle events
/// by a single instance object. 
/// </summary>
/// <remarks>
/// This class always gets the same single instance of handler.
/// </remarks>
/// <param name="handler"></param>
public class SingleInstanceHandlerFactory(IEventHandler handler) : IEventHandlerFactory
{
    /// <summary>
    /// The event handler instance.
    /// </summary>
    public IEventHandler HandlerInstance { get; } = handler;

    public IEventHandlerDisposeWrapper GetHandler() => new EventHandlerDisposeWrapper(HandlerInstance);

    public bool IsInFactories(List<IEventHandlerFactory> handlerFactories) => handlerFactories
            .OfType<SingleInstanceHandlerFactory>()
            .Any(f => f.HandlerInstance == HandlerInstance);
}