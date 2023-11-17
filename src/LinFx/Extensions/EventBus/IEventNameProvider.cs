namespace LinFx.Extensions.EventBus;

public interface IEventNameProvider
{
    string GetName(Type eventType);
}
