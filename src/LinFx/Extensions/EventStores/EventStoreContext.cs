using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinFx.Extensions.EventStores
{
    public class EventStoreContext : EntityFrameworkCore.DbContext
    {
        public EventStoreContext(DbContextOptions<EventStoreContext> options) : base(options)
        {
        }

        public DbSet<EventLog> IntegrationEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventLog>(ConfigureIntegrationEventLogEntry);
        }

        void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<EventLog> builder)
        {
            builder.ToTable("IntegrationEventLog");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

        }
    }
}
