using LinFx.Extensions.EventBus;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventStores
{
    public interface IEventStore
    {
        Task<IEnumerable<EventLog>> RetrieveEventLogsPendingToPublishAsync();
        Task SaveEventAsync(IntegrationEvent evt, DbTransaction transaction);
        Task MarkEventAsPublishedAsync(long eventId);
        Task MarkEventAsInProgressAsync(long eventId);
        Task MarkEventAsFailedAsync(long eventId);
    }
}
