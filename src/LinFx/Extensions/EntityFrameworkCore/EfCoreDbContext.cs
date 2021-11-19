﻿using LinFx.Data;
using LinFx.Domain.Entities;
using LinFx.Domain.Entities.Auditing;
using LinFx.Domain.Entities.Events;
using LinFx.Domain.Repositories;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.EntityFrameworkCore.EntityHistory;
using LinFx.Extensions.EntityFrameworkCore.Modeling;
using LinFx.Extensions.EntityFrameworkCore.ObjectExtending;
using LinFx.Extensions.EntityFrameworkCore.ValueConverters;
using LinFx.Extensions.EventBus.Distributed;
using LinFx.Extensions.EventBus.Local;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.ObjectExtending;
using LinFx.Extensions.Timing;
using LinFx.Extensions.Uow;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public abstract class EfCoreDbContext : DbContext, IEfCoreDbContext
    {
        public ILazyServiceProvider LazyServiceProvider { get; private set; }

        protected virtual string CurrentTenantId => CurrentTenant?.Id;

        protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

        protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

        public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

        public IEntityChangeEventHelper EntityChangeEventHelper => LazyServiceProvider.LazyGetService<IEntityChangeEventHelper>(NullEntityChangeEventHelper.Instance);

        public IAuditPropertySetter AuditPropertySetter => LazyServiceProvider.LazyGetRequiredService<IAuditPropertySetter>();

        public IEntityHistoryHelper EntityHistoryHelper => LazyServiceProvider.LazyGetService<IEntityHistoryHelper>(NullEntityHistoryHelper.Instance);

        public IAuditingManager AuditingManager => LazyServiceProvider.LazyGetRequiredService<IAuditingManager>();

        public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

        public IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

        public IDistributedEventBus DistributedEventBus => LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();

        public ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetRequiredService<ILocalEventBus>();

        public ILogger<EfCoreDbContext> Logger => LazyServiceProvider.LazyGetService<ILogger<EfCoreDbContext>>(NullLogger<EfCoreDbContext>.Instance);

        private static readonly MethodInfo ConfigureBasePropertiesMethodInfo = typeof(EfCoreDbContext)
            .GetMethod(nameof(ConfigureBaseProperties), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ConfigureValueConverterMethodInfo = typeof(EfCoreDbContext)
            .GetMethod(nameof(ConfigureValueConverter), BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo ConfigureValueGeneratedMethodInfo = typeof(EfCoreDbContext)
            .GetMethod(nameof(ConfigureValueGenerated), BindingFlags.Instance | BindingFlags.NonPublic);

        protected EfCoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TrySetDatabaseProvider(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureBasePropertiesMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                ConfigureValueConverterMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                ConfigureValueGeneratedMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        protected virtual void TrySetDatabaseProvider(ModelBuilder modelBuilder)
        {
            var provider = GetDatabaseProviderOrNull(modelBuilder);
            if (provider != null)
            {
                modelBuilder.SetDatabaseProvider(provider.Value);
            }
        }

        protected virtual EfCoreDatabaseProvider? GetDatabaseProviderOrNull(ModelBuilder modelBuilder)
        {
            switch (Database.ProviderName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    return EfCoreDatabaseProvider.SqlServer;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return EfCoreDatabaseProvider.PostgreSql;
                case "Pomelo.EntityFrameworkCore.MySql":
                    return EfCoreDatabaseProvider.MySql;
                case "Oracle.EntityFrameworkCore":
                case "Devart.Data.Oracle.Entity.EFCore":
                    return EfCoreDatabaseProvider.Oracle;
                case "Microsoft.EntityFrameworkCore.Sqlite":
                    return EfCoreDatabaseProvider.Sqlite;
                case "Microsoft.EntityFrameworkCore.InMemory":
                    return EfCoreDatabaseProvider.InMemory;
                case "FirebirdSql.EntityFrameworkCore.Firebird":
                    return EfCoreDatabaseProvider.Firebird;
                case "Microsoft.EntityFrameworkCore.Cosmos":
                    return EfCoreDatabaseProvider.Cosmos;
                default:
                    return null;
            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                var auditLog = AuditingManager?.Current?.Log;
                List<EntityChangeInfo> entityChangeList = null;
                if (auditLog != null)
                {
                    entityChangeList = EntityHistoryHelper.CreateChangeList(ChangeTracker.Entries().ToList());
                }

                ApplyConcepts();

                var eventReport = CreateEventReport();

                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                PublishEntityEvents(eventReport);

                if (entityChangeList != null)
                {
                    EntityHistoryHelper.UpdateChangeList(entityChangeList);
                    auditLog.EntityChanges.AddRange(entityChangeList);
                    Logger.LogDebug($"Added {entityChangeList.Count} entity changes to the current audit log");
                }

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message, ex);
            }
            finally
            {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        private void PublishEntityEvents(EntityEventReport changeReport)
        {
            foreach (var localEvent in changeReport.DomainEvents)
            {
                UnitOfWorkManager.Current?.AddOrReplaceLocalEvent(new UnitOfWorkEventRecord(localEvent.EventData.GetType(), localEvent.EventData, localEvent.EventOrder));
            }

            foreach (var distributedEvent in changeReport.DistributedEvents)
            {
                UnitOfWorkManager.Current?.AddOrReplaceDistributedEvent(new UnitOfWorkEventRecord(distributedEvent.EventData.GetType(), distributedEvent.EventData, distributedEvent.EventOrder));
            }
        }

        /// <summary>
        /// This method will call the DbContext <see cref="SaveChangesAsync(bool, CancellationToken)"/> method directly of EF Core, which doesn't apply concepts of abp.
        /// </summary>
        public virtual Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public virtual void Initialize(EfCoreDbContextInitializationContext initializationContext)
        {
            LazyServiceProvider = initializationContext.UnitOfWork.ServiceProvider.GetRequiredService<ILazyServiceProvider>();

            if (initializationContext.UnitOfWork.Options.Timeout.HasValue &&
                Database.IsRelational() &&
                !Database.GetCommandTimeout().HasValue)
            {
                Database.SetCommandTimeout(TimeSpan.FromMilliseconds(initializationContext.UnitOfWork.Options.Timeout.Value));
            }

            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.Tracked += ChangeTracker_Tracked;
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        }

        protected virtual void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
        {
            FillExtraPropertiesForTrackedEntities(e);
            PublishEventsForTrackedEntity(e.Entry);
        }

        protected virtual void ChangeTracker_StateChanged(object sender, EntityStateChangedEventArgs e)
        {
            PublishEventsForTrackedEntity(e.Entry);
        }

        protected virtual void FillExtraPropertiesForTrackedEntities(EntityTrackedEventArgs e)
        {
            var entityType = e.Entry.Metadata.ClrType;
            if (entityType == null)
                return;

            if (!(e.Entry.Entity is IHasExtraProperties entity))
                return;

            if (!e.FromQuery)
                return;

            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension == null)
                return;

            foreach (var property in objectExtension.GetProperties())
            {
                if (!property.IsMappedToFieldForEfCore())
                    continue;

                /* Checking "currentValue != null" has a good advantage:
                 * Assume that you we already using a named extra property,
                 * then decided to create a field (entity extension) for it.
                 * In this way, it prevents to delete old value in the JSON and
                 * updates the field on the next save!
                 */

                var currentValue = e.Entry.CurrentValues[property.Name];
                if (currentValue != null)
                    entity.ExtraProperties[property.Name] = currentValue;
            }
        }

        private void PublishEventsForTrackedEntity(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    EntityChangeEventHelper.PublishEntityCreatingEvent(entry.Entity);
                    EntityChangeEventHelper.PublishEntityCreatedEvent(entry.Entity);
                    break;
                case EntityState.Modified:
                    if (entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
                    {
                        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                        {
                            EntityChangeEventHelper.PublishEntityDeletingEvent(entry.Entity);
                            EntityChangeEventHelper.PublishEntityDeletedEvent(entry.Entity);
                        }
                        else
                        {
                            EntityChangeEventHelper.PublishEntityUpdatingEvent(entry.Entity);
                            EntityChangeEventHelper.PublishEntityUpdatedEvent(entry.Entity);
                        }
                    }
                    break;
                case EntityState.Deleted:
                    EntityChangeEventHelper.PublishEntityDeletingEvent(entry.Entity);
                    EntityChangeEventHelper.PublishEntityDeletedEvent(entry.Entity);
                    break;
            }
        }

        protected virtual void ApplyConcepts()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                ApplyConcepts(entry);
            }
        }

        protected virtual EntityEventReport CreateEventReport()
        {
            var eventReport = new EntityEventReport();

            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                var generatesDomainEventsEntity = entry.Entity as IGeneratesDomainEvents;
                if (generatesDomainEventsEntity == null)
                    continue;

                var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
                if (localEvents != null && localEvents.Any())
                {
                    eventReport.DomainEvents.AddRange(localEvents.Select(eventRecord => new DomainEventEntry(entry.Entity, eventRecord.EventData, eventRecord.EventOrder)));
                    generatesDomainEventsEntity.ClearLocalEvents();
                }

                var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
                if (distributedEvents != null && distributedEvents.Any())
                {
                    eventReport.DistributedEvents.AddRange(distributedEvents.Select(eventRecord => new DomainEventEntry(entry.Entity, eventRecord.EventData, eventRecord.EventOrder)));
                    generatesDomainEventsEntity.ClearDistributedEvents();
                }
            }

            return eventReport;
        }

        protected virtual void ApplyConcepts(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyConceptsForAddedEntity(entry);
                    break;
                case EntityState.Modified:
                    ApplyConceptsForModifiedEntity(entry);
                    break;
                case EntityState.Deleted:
                    ApplyConceptsForDeletedEntity(entry);
                    break;
            }

            HandleExtraPropertiesOnSave(entry);
        }

        protected virtual void HandleExtraPropertiesOnSave(EntityEntry entry)
        {
            if (entry.State.IsIn(EntityState.Deleted, EntityState.Unchanged))
                return;

            var entityType = entry.Metadata.ClrType;
            if (entityType == null)
                return;

            if (!(entry.Entity is IHasExtraProperties entity))
                return;

            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension == null)
                return;

            var efMappedProperties = ObjectExtensionManager.Instance
                .GetProperties(entityType)
                .Where(p => p.IsMappedToFieldForEfCore());

            //foreach (var property in efMappedProperties)
            //{
            //    if (!entity.HasProperty(property.Name))
            //    {
            //        continue;
            //    }

            //    var entryProperty = entry.Property(property.Name);
            //    var entityProperty = entity.GetProperty(property.Name);
            //    if (entityProperty == null)
            //    {
            //        entryProperty.CurrentValue = null;
            //        continue;
            //    }

            //    if (entryProperty.Metadata.ClrType == entityProperty.GetType())
            //    {
            //        entryProperty.CurrentValue = entityProperty;
            //    }
            //    else
            //    {
            //        if (TypeHelper.IsPrimitiveExtended(entryProperty.Metadata.ClrType, includeEnums: true))
            //        {
            //            var conversionType = entryProperty.Metadata.ClrType;
            //            if (TypeHelper.IsNullable(conversionType))
            //            {
            //                conversionType = conversionType.GetFirstGenericArgumentIfNullable();
            //            }

            //            if (conversionType == typeof(Guid))
            //            {
            //                entryProperty.CurrentValue = TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(entityProperty.ToString());
            //            }
            //            else if (conversionType.IsEnum)
            //            {
            //                entryProperty.CurrentValue = Enum.Parse(conversionType, entityProperty.ToString(), ignoreCase: true);
            //            }
            //            else
            //            {
            //                entryProperty.CurrentValue = Convert.ChangeType(entityProperty, conversionType, CultureInfo.InvariantCulture);
            //            }
            //        }
            //    }
            //}
        }

        protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry)
        {
            CheckAndSetId(entry);
            SetConcurrencyStampIfNull(entry);
            SetCreationAuditProperties(entry);
        }

        protected virtual void ApplyConceptsForModifiedEntity(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
            {
                UpdateConcurrencyStamp(entry);
                SetModificationAuditProperties(entry);

                if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                {
                    SetDeletionAuditProperties(entry);
                }
            }
        }

        protected virtual void ApplyConceptsForDeletedEntity(EntityEntry entry)
        {
            if (TryCancelDeletionForSoftDelete(entry))
            {
                UpdateConcurrencyStamp(entry);
                SetDeletionAuditProperties(entry);
            }
        }

        protected virtual bool IsHardDeleted(EntityEntry entry)
        {
            var hardDeletedEntities = UnitOfWorkManager?.Current?.Items.GetOrDefault(UnitOfWorkItemNames.HardDeletedEntities) as HashSet<IEntity>;
            if (hardDeletedEntities == null)
                return false;

            return hardDeletedEntities.Contains(entry.Entity);
        }

        protected virtual void UpdateConcurrencyStamp(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
                return;

            Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual void SetConcurrencyStampIfNull(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
                return;

            if (entity.ConcurrencyStamp != null)
                return;

            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual bool TryCancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
                return false;

            if (IsHardDeleted(entry))
                return false;

            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
            return true;
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            if (entry.Entity is IEntity<string> entityWithId)
                TrySetId(entry, entityWithId);
        }

        protected virtual void TrySetId(EntityEntry entry, IEntity<string> entity)
        {
            if (entity.Id != default)
                return;

            var idProperty = entry.Property("Id").Metadata.PropertyInfo;

            //Check for DatabaseGeneratedAttribute
            var dbGeneratedAttr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
            if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
                return;

            EntityHelper.TrySetId(entity, IDUtils.NewIdString(), true);
        }

        protected virtual void SetCreationAuditProperties(EntityEntry entry)
        {
            AuditPropertySetter?.SetCreationProperties(entry.Entity);
        }

        protected virtual void SetModificationAuditProperties(EntityEntry entry)
        {
            AuditPropertySetter?.SetModificationProperties(entry.Entity);
        }

        protected virtual void SetDeletionAuditProperties(EntityEntry entry)
        {
            AuditPropertySetter?.SetDeletionProperties(entry.Entity);
        }

        protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType) where TEntity : class
        {
            if (mutableEntityType.IsOwned())
                return;

            if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
                return;

            modelBuilder.Entity<TEntity>().ConfigureByConvention();

            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
        }

        protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType) where TEntity : class
        {
            if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
            }
        }

        protected virtual void ConfigureValueConverter<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType) where TEntity : class
        {
            if (mutableEntityType.BaseType == null &&
                !typeof(TEntity).IsDefined(typeof(DisableDateTimeNormalizationAttribute), true) &&
                !typeof(TEntity).IsDefined(typeof(OwnedAttribute), true) &&
                !mutableEntityType.IsOwned())
            {
                if (LazyServiceProvider == null || Clock == null || !Clock.SupportsMultipleTimezone)
                    return;

                var dateTimeValueConverter = new DateTimeValueConverter(Clock);
                var dateTimePropertyInfos = typeof(TEntity).GetProperties()
                    .Where(property =>
                        (property.PropertyType == typeof(DateTime) ||
                         property.PropertyType == typeof(DateTime?)) &&
                        property.CanWrite &&
                        !property.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true)
                    ).ToList();

                dateTimePropertyInfos.ForEach(property =>
                {
                    modelBuilder
                        .Entity<TEntity>()
                        .Property(property.Name)
                        .HasConversion(dateTimeValueConverter);
                });
            }
        }

        protected virtual void ConfigureValueGenerated<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType) where TEntity : class
        {
            if (!typeof(IEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
                return;

            var idPropertyBuilder = modelBuilder.Entity<TEntity>().Property(x => ((IEntity<Guid>)x).Id);
            if (idPropertyBuilder.Metadata.PropertyInfo.IsDefined(typeof(DatabaseGeneratedAttribute), true))
                return;

            idPropertyBuilder.ValueGeneratedNever();
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
                return true;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                return true;

            return false;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>() where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                expression = e => !IsSoftDeleteFilterEnabled || !EF.Property<bool>(e, "IsDeleted");

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> multiTenantFilter = e => !IsMultiTenantFilterEnabled || EF.Property<string>(e, "TenantId") == CurrentTenantId;
                expression = expression == null ? multiTenantFilter : CombineExpressions(expression, multiTenantFilter);
            }

            return expression;
        }

        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;

                return base.Visit(node);
            }
        }
    }
}
