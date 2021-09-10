using System.Collections.Generic;
using LinFx.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinFx.EntityFrameworkCore.EntityHistory
{
    public interface IEntityHistoryHelper
    {
        List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);

        void UpdateChangeList(List<EntityChangeInfo> entityChanges);
    }
}
