﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.Uow
{
    public interface IUnitOfWorkEventPublisher
    {
        Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents);

        Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents);
    }
}
