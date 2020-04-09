using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.EventStores
{
    public class EventStoreOptionsBuilder
    {
        public virtual EventStoreOptions Options { get; }

        public EventStoreOptionsBuilder(
            [NotNull]EventStoreOptions options)
        {
            Check.NotNull(options, nameof(options));

            Options = options;
        }

        public virtual EventStoreOptionsBuilder ConfigureDbContext(
            [NotNull] Action<DbContextOptionsBuilder> optionsAction)
            => ConfigureDbContext<EventStoreContext>(optionsAction);

        public virtual EventStoreOptionsBuilder ConfigureDbContext<TContext>(
            [NotNull] Action<DbContextOptionsBuilder> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            optionsAction.Invoke(new DbContextOptionsBuilder());

            return this;
        }
    }
}