using System;

namespace LinFx.Domain.Entities.Events.Distributed
{
    public class EtoMappingDictionaryItem
    {
        public Type EtoType { get; }

        public Type ObjectMappingContextType { get; }

        public EtoMappingDictionaryItem(Type etoType, Type objectMappingContextType = null)
        {
            EtoType = etoType;
            ObjectMappingContextType = objectMappingContextType;
        }
    }
}