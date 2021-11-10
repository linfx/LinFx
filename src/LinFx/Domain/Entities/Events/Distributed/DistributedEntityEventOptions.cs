namespace LinFx.Domain.Entities.Events.Distributed
{
    public class DistributedEntityEventOptions
    {
        public IAutoEntityDistributedEventSelectorList AutoEventSelectors { get; }
        
        public EtoMappingDictionary EtoMappings { get; set; }

        public DistributedEntityEventOptions()
        {
            AutoEventSelectors = new AutoEntityDistributedEventSelectorList();
            EtoMappings = new EtoMappingDictionary();
        }
    }
}