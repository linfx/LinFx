using System;

namespace LinFx.Extensions.EventBus;

public class EventBusOptions
{
    public bool EnabledErrorHandle { get; set; }

    public Func<Type, bool> ErrorHandleSelector { get; set; }

    public string DeadLetterName { get; set; }

    public EventBusRetryStrategyOptions RetryStrategyOptions { get; set; }

    public void UseRetryStrategy(Action<EventBusRetryStrategyOptions> action = null)
    {
        EnabledErrorHandle = true;
        RetryStrategyOptions = new EventBusRetryStrategyOptions();
        action?.Invoke(RetryStrategyOptions);
    }
}
