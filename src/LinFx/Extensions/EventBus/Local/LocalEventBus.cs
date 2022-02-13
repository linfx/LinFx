using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.Threading;
using LinFx.Extensions.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Local
{
    /// <summary>
    /// Implements EventBus as Singleton pattern.
    /// </summary>
    public class LocalEventBus : EventBusBase, ILocalEventBus
    {
        /// <summary>
        /// Reference to the Logger.
        /// </summary>
        public ILogger Logger { get; set; }

        protected LocalEventBusOptions Options { get; }

        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }

        public LocalEventBus(
            IOptions<LocalEventBusOptions> options,
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IEventErrorHandler errorHandler)
            : base(serviceScopeFactory, currentTenant, unitOfWorkManager, errorHandler)
        {
            Options = options.Value;
            Logger = NullLogger<LocalEventBus>.Instance;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            SubscribeHandlers(Options.Handlers);
        }

        /// <inheritdoc/>
        public virtual IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        /// <inheritdoc/>
        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories =>
            {
                if (!factory.IsInFactories(factories))
                {
                    factories.Add(factory);
                }
            });

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        /// <inheritdoc/>
        public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEvent)).Locking(factories =>
            {
                factories.RemoveAll(factory =>
                {
                    if (factory is not SingleInstanceHandlerFactory singleInstanceFactory)
                        return false;

                    if (singleInstanceFactory.HandlerInstance is not ActionEventHandler<TEvent> actionHandler)
                        return false;

                    return actionHandler.Action == action;
                });
            });
        }

        /// <inheritdoc/>
        public override void Unsubscribe(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories =>
            {
                factories.RemoveAll(factory => factory is SingleInstanceHandlerFactory && (factory as SingleInstanceHandlerFactory).HandlerInstance == handler);
            });
        }

        /// <inheritdoc/>
        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        /// <inheritdoc/>
        public override void UnsubscribeAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
        }

        protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
        {
            await PublishAsync(new LocalEventMessage(Guid.NewGuid(), eventData, eventType));
        }

        protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
        {
            unitOfWork.AddOrReplaceLocalEvent(eventRecord);
        }

        /// <summary>
        /// ·¢²¼
        /// </summary>
        /// <param name="localEventMessage"></param>
        /// <returns></returns>
        public virtual async Task PublishAsync(LocalEventMessage localEventMessage)
        {
            await TriggerHandlersAsync(localEventMessage.EventType, localEventMessage.EventData, errorContext =>
            {
                errorContext.EventData = localEventMessage.EventData;
                //errorContext.SetProperty(nameof(LocalEventMessage.MessageId), localEventMessage.MessageId);
            });
        }

        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
                return true;

            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
                return true;

            return false;
        }
    }
}
