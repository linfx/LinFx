using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ObjectMapping;
using LinFx.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace LinFx.Domain.Entities.Events.Distributed;

[Service]
public class EntityToEtoMapper : IEntityToEtoMapper
{
    protected IServiceScopeFactory HybridServiceScopeFactory { get; }

    protected DistributedEntityEventOptions Options { get; }

    public EntityToEtoMapper(
        IOptions<DistributedEntityEventOptions> options,
        IServiceScopeFactory hybridServiceScopeFactory)
    {
        HybridServiceScopeFactory = hybridServiceScopeFactory;
        Options = options.Value;
    }

    public object Map(object entityObj)
    {
        Check.NotNull(entityObj, nameof(entityObj));

        if (entityObj is not IEntity entity)
            return null;

        var entityType = ProxyHelper.UnProxy(entity).GetType();
        var etoMappingItem = Options.EtoMappings.GetOrDefault(entityType);
        if (etoMappingItem == null)
        {
            var keys = entity.GetKeys().JoinAsString(",");
            return new EntityEto(entityType.FullName, keys);
        }

        using (var scope = HybridServiceScopeFactory.CreateScope())
        {
            var objectMapperType = etoMappingItem.ObjectMappingContextType == null
                ? typeof(IObjectMapper)
                : typeof(IObjectMapper<>).MakeGenericType(etoMappingItem.ObjectMappingContextType);

            var objectMapper = (IObjectMapper)scope.ServiceProvider.GetRequiredService(objectMapperType);

            return objectMapper.Map(entityType, etoMappingItem.EtoType, entityObj);
        }
    }
}
