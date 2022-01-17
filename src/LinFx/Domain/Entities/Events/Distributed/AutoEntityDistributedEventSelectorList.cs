using System.Collections.Generic;

namespace LinFx.Domain.Entities.Events.Distributed;

public class AutoEntityDistributedEventSelectorList : List<NamedTypeSelector>, IAutoEntityDistributedEventSelectorList
{
}