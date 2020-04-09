using LinFx.Extensions.EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventStores
{
    public class EventStoreManager : IEventStore
    {
        private readonly EventStoreOptions _options;
        private readonly EventStoreContext _context;
        private readonly List<Type> _eventTypes;

        public EventStoreManager(
            [NotNull] IOptions<EventStoreOptions> options
            /*[NotNull] EventStoreContext context*/)
        {
            Check.NotNull(options, nameof(options));
            //Check.NotNull(context, nameof(context));

            _options = options.Value;
            //_context = context;
            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<EventLog>> RetrieveEventLogsPendingToPublishAsync()
        {
            return await _context.EventLogs
                .Where(e => e.State == EventStateEnum.NotPublished)
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)))
                .ToListAsync();
        }

        public Task SaveEventAsync(IntegrationEvent evt, DbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");

            var eventLogEntry = new EventLog(evt);

            _context.Database.UseTransaction(transaction);
            _context.EventLogs.Add(eventLogEntry);
            return _context.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(long eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(long eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(long eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private Task UpdateEventStatus(long eventId, EventStateEnum status)
        {
            var eventLogEntry = _context.EventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _context.EventLogs.Update(eventLogEntry);
            return _context.SaveChangesAsync();
        }
    }
}