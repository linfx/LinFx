using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LinFx.Extensions.MongoDB;

public class MongoDbContextOptions
{
    internal Dictionary<Type, Type> DbContextReplacements { get; }

    public Action<MongoClientSettings> MongoClientSettingsConfigurer { get; set; }

    public MongoDbContextOptions()
    {
        DbContextReplacements = new Dictionary<Type, Type>();
    }

    internal Type GetReplacedTypeOrSelf(Type dbContextType)
    {
        var replacementType = dbContextType;
        while (true)
        {
            if (DbContextReplacements.TryGetValue(replacementType, out var foundType))
            {
                if (foundType == dbContextType)
                {
                    throw new Exception(
                        "Circular DbContext replacement found for " +
                        dbContextType.AssemblyQualifiedName
                    );
                }

                replacementType = foundType;
            }
            else
            {
                return replacementType;
            }
        }
    }
}
