using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.RabbitMq
{
    [Service(Lifetime = ServiceLifetime.Singleton)]
    public class RabbitMqEventErrorHandler : EventErrorHandlerBase
    {
        public RabbitMqEventErrorHandler(
            IOptions<EventBusOptions> options)
            : base(options)
        {
        }

        protected override async Task RetryAsync(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            context.TryGetRetryAttempt(out var retryAttempt);

            //await context.EventBus.As<RabbitMqDistributedEventBus>().PublishAsync(
            //    context.EventType,
            //    context.EventData,
            //    context.GetProperty(HeadersKey).As<IBasicProperties>(),
            //    new Dictionary<string, object>
            //    {
            //        {RetryAttemptKey, ++retryAttempt},
            //        {"exceptions", context.Exceptions.Select(x => x.ToString()).ToList()}
            //    });
        }

        protected override Task MoveToDeadLetterAsync(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }
    }
}
