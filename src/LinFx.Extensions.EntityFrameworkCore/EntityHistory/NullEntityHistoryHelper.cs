using LinFx.Extensions.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinFx.Extensions.EntityFrameworkCore.EntityHistory;

public class NullEntityHistoryHelper : IEntityHistoryHelper
{
    public static NullEntityHistoryHelper Instance { get; } = new NullEntityHistoryHelper();

    public List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
    {
        return new List<EntityChangeInfo>();
    }

    public void UpdateChangeList(List<EntityChangeInfo> entityChanges)
    {
    }
}
