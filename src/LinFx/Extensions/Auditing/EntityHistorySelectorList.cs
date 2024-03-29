namespace LinFx.Extensions.Auditing;

internal class EntityHistorySelectorList : List<NamedTypeSelector>, IEntityHistorySelectorList
{
    public bool RemoveByName(string name) => RemoveAll(s => s.Name == name) > 0;
}
