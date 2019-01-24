using LinFx.Extensions.EventBus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LinFx.Extensions.IntegrationEventLog
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _context;
        private readonly List<Type> _eventTypes;

        public IntegrationEventLogService(IntegrationEventLogContext context)
        {
            _context = context;
            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            return await _context.IntegrationEventLogs
                .Where(e => e.State == EventStateEnum.NotPublished)
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)))
                .ToListAsync();
        }

        public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");
            }

            var eventLogEntry = new IntegrationEventLogEntry(@event);

            _context.Database.UseTransaction(transaction);
            _context.IntegrationEventLogs.Add(eventLogEntry);

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
            var eventLogEntry = _context.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _context.IntegrationEventLogs.Update(eventLogEntry);

            return _context.SaveChangesAsync();
        }
    }
}
