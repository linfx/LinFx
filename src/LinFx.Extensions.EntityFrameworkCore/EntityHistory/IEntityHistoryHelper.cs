using System.Collections.Generic;
using LinFx.Extensions.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinFx.Extensions.EntityFrameworkCore.EntityHistory;

public interface IEntityHistoryHelper
{
    List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}
