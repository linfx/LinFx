using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Local
{
    //[ExposeServices(typeof(LocalEventErrorHandler), typeof(IEventErrorHandler))]
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class LocalEventErrorHandler : EventErrorHandlerBase
    {
        protected Dictionary<Guid, int> RetryTracking { get; }

        public LocalEventErrorHandler(
            IOptions<EventBusOptions> options)
            : base(options)
        {
            RetryTracking = new Dictionary<Guid, int>();
        }

        protected override async Task RetryAsync(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            //var messageId = context.GetProperty<Guid>(nameof(LocalEventMessage.MessageId));

            //context.TryGetRetryAttempt(out var retryAttempt);
            //RetryTracking[messageId] = ++retryAttempt;

            //await context.EventBus.As<LocalEventBus>().PublishAsync(new LocalEventMessage(messageId, context.EventData, context.EventType));

            //RetryTracking.Remove(messageId);
        }

        protected override Task MoveToDeadLetterAsync(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }

        protected override async Task<bool> ShouldRetryAsync(EventExecutionErrorContext context)
        {
            //var messageId = context.GetProperty<Guid>(nameof(LocalEventMessage.MessageId));
            //context.SetProperty(RetryAttemptKey, RetryTracking.GetOrDefault(messageId));

            //if (await base.ShouldRetryAsync(context))
            //{
            //    return true;
            //}

            //RetryTracking.Remove(messageId);
            return false;
        }
    }
}
