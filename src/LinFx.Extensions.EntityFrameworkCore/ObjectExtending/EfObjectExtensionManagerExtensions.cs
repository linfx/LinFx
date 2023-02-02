using JetBrains.Annotations;
using LinFx.Domain.Entities;
using LinFx.Extensions.ObjectExtending;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LinFx.Extensions.EntityFrameworkCore.ObjectExtending
{
    public static class EfObjectExtensionManagerExtensions
    {
        public static ObjectExtensionManager MapEfDbContext<TDbContext>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Action<ModelBuilder> modelBuilderAction)
            where TDbContext : DbContext
        {
            return objectExtensionManager.AddOrUpdate(
                typeof(TDbContext),
                options =>
                {
                    options.MapEfeDbContext(modelBuilderAction);
                });
        }

        public static ObjectExtensionManager MapEfEntity<TEntity>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
            where TEntity : IEntity
        {
            return objectExtensionManager.MapEfEntity(
                typeof(TEntity),
                entityTypeBuildAction);
        }

        public static ObjectExtensionManager MapEfEntity(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type entityType,
            [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdate(
                entityType,
                options =>
                {
                    options.MapEfEntity(entityTypeBuildAction);
                });
        }

        public static ObjectExtensionManager MapEfProperty<TEntity, TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName)
            where TEntity : IHasExtraProperties, IEntity
        {
            return objectExtensionManager.MapEfProperty(
                typeof(TEntity),
                typeof(TProperty),
                propertyName
            );
        }

        public static ObjectExtensionManager MapEfProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type entityType,
            [NotNull] Type propertyType,
            [NotNull] string propertyName)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdateProperty(
                entityType,
                propertyType,
                propertyName,
                options => { options.MapEf(); }
            );
        }

        public static ObjectExtensionManager MapEfProperty<TEntity, TProperty>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
            where TEntity : IHasExtraProperties, IEntity
        {
            return objectExtensionManager.MapEfProperty(
                typeof(TEntity),
                typeof(TProperty),
                propertyName,
                entityTypeAndPropertyBuildAction
            );
        }

        public static ObjectExtensionManager MapEfProperty(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] Type entityType,
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));

            return objectExtensionManager.AddOrUpdateProperty(
                entityType,
                propertyType,
                propertyName,
                options =>
                {
                    options.MapEf(
                        entityTypeAndPropertyBuildAction
                    );
                }
            );
        }

        public static void ConfigureEfEntity(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] EntityTypeBuilder typeBuilder)
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
            Check.NotNull(typeBuilder, nameof(typeBuilder));

            var objectExtension = objectExtensionManager.GetOrNull(typeBuilder.Metadata.ClrType);
            if (objectExtension == null)
            {
                return;
            }

            var efEntityMappings = objectExtension.GetEfEntityMappings();

            foreach (var efEntityMapping in efEntityMappings)
            {
                efEntityMapping.EntityTypeBuildAction?.Invoke(typeBuilder);
            }

            foreach (var property in objectExtension.GetProperties())
            {
                var efMapping = property.GetEfMappingOrNull();
                if (efMapping == null)
                {
                    continue;
                }

                /* Prevent multiple calls to the entityTypeBuilder.Property(...) method */
                if (typeBuilder.Metadata.FindProperty(property.Name) != null)
                {
                    continue;
                }

                var propertyBuilder = typeBuilder.Property(property.Type, property.Name);

                efMapping.EntityTypeAndPropertyBuildAction?.Invoke(typeBuilder, propertyBuilder);
#pragma warning disable 618
                efMapping.PropertyBuildAction?.Invoke(propertyBuilder);
#pragma warning restore 618
            }
        }

        public static void ConfigureEfDbContext<TDbContext>(
            [NotNull] this ObjectExtensionManager objectExtensionManager,
            [NotNull] ModelBuilder modelBuilder)
            where TDbContext : DbContext
        {
            Check.NotNull(objectExtensionManager, nameof(objectExtensionManager));
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            var objectExtension = objectExtensionManager.GetOrNull(typeof(TDbContext));
            if (objectExtension == null)
            {
                return;
            }

            var efDbContextMappings = objectExtension.GetEfDbContextMappings();

            foreach (var efDbContextMapping in efDbContextMappings)
            {
                efDbContextMapping.ModelBuildAction?.Invoke(modelBuilder);
            }
        }
    }
}
