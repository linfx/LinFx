using System.Collections.Generic;
using LinFx.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinFx.Extensions.EntityFrameworkCore.EntityHistory
{
    public class NullEntityHistoryHelper : IEntityHistoryHelper
    {
        public static NullEntityHistoryHelper Instance { get; } = new NullEntityHistoryHelper();

        private NullEntityHistoryHelper()
        {
        }

        public List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
        {
            return new List<EntityChangeInfo>();
        }

        public void UpdateChangeList(List<EntityChangeInfo> entityChanges)
        {
        }
    }
}