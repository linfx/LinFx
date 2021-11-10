using LinFx;
using LinFx.Extensions.Auditing;
using System.Collections.Generic;

namespace LinFx.Domain.Entities.Auditing
{
    internal class EntityHistorySelectorList : List<NamedTypeSelector>, IEntityHistorySelectorList
    {
        public bool RemoveByName(string name)
        {
            return RemoveAll(s => s.Name == name) > 0;
        }
    }
}
