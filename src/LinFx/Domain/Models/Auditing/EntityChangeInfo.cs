using System.Collections.Generic;

namespace LinFx.Domain.Models.Auditing
{
    public class EntityChangeInfo
    {
        public IEnumerable<object> PropertyChanges { get; internal set; }
        public object ChangeType { get; internal set; }
        public object EntityTypeFullName { get; internal set; }
        public object EntityId { get; internal set; }
    }
}