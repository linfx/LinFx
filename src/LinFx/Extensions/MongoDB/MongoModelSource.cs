using LinFx.Domain.Models;
using LinFx.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace LinFx.Extensions.MongoDB
{
    public class MongoModelSource : IMongoModelSource
    {
        protected readonly ConcurrentDictionary<Type, MongoDbContextModel> ModelCache = new ConcurrentDictionary<Type, MongoDbContextModel>();

        public MongoDbContextModel GetModel(MongoDbContext dbContext)
        {
            return ModelCache.GetOrAdd(dbContext.GetType(), _ => CreateModel(dbContext));
        }

        protected virtual MongoDbContextModel CreateModel(MongoDbContext dbContext)
        {
            var modelBuilder = CreateModelBuilder();
            BuildModelFromDbContextType(modelBuilder, dbContext.GetType());
            BuildModelFromDbContextInstance(modelBuilder, dbContext);
            return modelBuilder.Build();
        }

        protected virtual MongoModelBuilder CreateModelBuilder()
        {
            return new MongoModelBuilder();
        }

        protected virtual void BuildModelFromDbContextType(IMongoModelBuilder modelBuilder, Type dbContextType)
        {
            var collectionProperties =
                from property in dbContextType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    ReflectionUtils.IsAssignableToGenericType(property.PropertyType, typeof(IMongoCollection<>)) &&
                    typeof(IEntity).IsAssignableFrom(property.PropertyType.GenericTypeArguments[0])
                select property;

            foreach (var collectionProperty in collectionProperties)
            {
                BuildModelFromDbContextCollectionProperty(modelBuilder, collectionProperty);
            }
        }

        protected virtual void BuildModelFromDbContextCollectionProperty(IMongoModelBuilder modelBuilder, PropertyInfo collectionProperty)
        {
            var entityType = collectionProperty.PropertyType.GenericTypeArguments[0];
            var collectionAttribute = collectionProperty.GetCustomAttributes().OfType<MongoCollectionAttribute>().FirstOrDefault();

            modelBuilder.Entity(entityType, b =>
            {
                b.CollectionName = collectionAttribute?.CollectionName ?? collectionProperty.Name;
            });
        }

        protected virtual void BuildModelFromDbContextInstance(IMongoModelBuilder modelBuilder, MongoDbContext dbContext)
        {
            dbContext.CreateModel(modelBuilder);
        }
    }
}
