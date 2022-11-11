using LinFx.Domain.Entities;
using LinFx.Domain.Entities.Events;
using LinFx.Domain.Repositories;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.Data;
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
using LinFx.Reflection;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace LinFx.Extensions.EntityFrameworkCore;

/// <summary>
/// 数据库上下文
/// </summary>
public abstract class EfDbContext : DbContext, IEfDbContext, ITransientDependency
{
    [Autowired]
    public ILazyServiceProvider LazyServiceProvider { get; set; }

    /// <summary>
    /// 当前租户ID
    /// </summary>
    protected virtual string? CurrentTenantId => CurrentTenant?.Id;

    /// <summary>
    /// 是否启用租户过滤
    /// </summary>
    protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

    /// <summary>
    /// 是否启用软件删除过滤
    /// </summary>
    protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

    /// <summary>
    /// 当前租户
    /// </summary>
    public ICurrentTenant? CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    /// <summary>
    /// 数据过滤
    /// </summary>
    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    public IEntityChangeEventHelper EntityChangeEventHelper => LazyServiceProvider.LazyGetService<IEntityChangeEventHelper>(NullEntityChangeEventHelper.Instance);

    /// <summary>
    /// 属性自动设置器
    /// </summary>
    public IAuditPropertySetter AuditPropertySetter => LazyServiceProvider.LazyGetRequiredService<IAuditPropertySetter>();

    public IEntityHistoryHelper EntityHistoryHelper => LazyServiceProvider.LazyGetService<IEntityHistoryHelper>(NullEntityHistoryHelper.Instance);

    /// <summary>
    /// 审记日志管理器
    /// </summary>
    public IAuditingManager AuditingManager => LazyServiceProvider.LazyGetRequiredService<IAuditingManager>();

    /// <summary>
    /// 工作单元管理器
    /// </summary>
    public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    /// <summary>
    /// 系统时钟
    /// </summary>
    public IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    /// <summary>
    /// 分步式事件总线
    /// </summary>
    public IDistributedEventBus DistributedEventBus => LazyServiceProvider.LazyGetRequiredService<IDistributedEventBus>();

    /// <summary>
    /// 本地事件总线
    /// </summary>
    public ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetRequiredService<ILocalEventBus>();

    /// <summary>
    /// 日志
    /// </summary>
    public ILogger Logger => LazyServiceProvider.LazyGetService<ILogger<EfDbContext>>(NullLogger<EfDbContext>.Instance);

    private static readonly MethodInfo? ConfigureBasePropertiesMethodInfo = typeof(EfDbContext)
        .GetMethod(nameof(ConfigureBaseProperties), BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly MethodInfo? ConfigureValueConverterMethodInfo = typeof(EfDbContext)
        .GetMethod(nameof(ConfigureValueConverter), BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly MethodInfo? ConfigureValueGeneratedMethodInfo = typeof(EfDbContext)
        .GetMethod(nameof(ConfigureValueGenerated), BindingFlags.Instance | BindingFlags.NonPublic);

    protected EfDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBasePropertiesMethodInfo?
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });

            ConfigureValueConverterMethodInfo?
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });

            ConfigureValueGeneratedMethodInfo?
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
        }
    }

    /// <summary>
    /// 设置数据库提供者
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected virtual void TrySetDatabaseProvider(ModelBuilder modelBuilder)
    {
        var provider = GetDatabaseProviderOrNull(modelBuilder);
        if (provider != null)
            modelBuilder.SetDatabaseProvider(provider.Value);
    }

    protected virtual EfDatabaseProvider? GetDatabaseProviderOrNull(ModelBuilder modelBuilder) => Database.ProviderName switch
    {
        "Microsoft.EntityFrameworkCore.SqlServer" => EfDatabaseProvider.SqlServer,
        "Npgsql.EntityFrameworkCore.PostgreSQL" => EfDatabaseProvider.PostgreSql,
        "Pomelo.EntityFrameworkCore.MySql" => EfDatabaseProvider.MySql,
        "Oracle.EntityFrameworkCore" or "Devart.Data.Oracle.Entity.EFCore" => EfDatabaseProvider.Oracle,
        "Microsoft.EntityFrameworkCore.Sqlite" => EfDatabaseProvider.Sqlite,
        "Microsoft.EntityFrameworkCore.InMemory" => EfDatabaseProvider.InMemory,
        "FirebirdSql.EntityFrameworkCore.Firebird" => EfDatabaseProvider.Firebird,
        "Microsoft.EntityFrameworkCore.Cosmos" => EfDatabaseProvider.Cosmos,
        _ => null,
    };

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="DbConcurrencyException"></exception>
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        try
        {
            List<EntityChangeInfo>? entityChangeList = null;
            var auditLog = AuditingManager?.Current?.Log;
            if (auditLog != null)
                entityChangeList = EntityHistoryHelper.CreateChangeList(ChangeTracker.Entries().ToList());

            // 保存前属性处理
            HandlePropertiesBeforeSave();

            // 创建领域事件
            var eventReport = CreateEventReport();

            // 保存
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // 发布领域事件
            PublishEntityEvents(eventReport);

            if (entityChangeList != null)
            {
                EntityHistoryHelper.UpdateChangeList(entityChangeList);
                auditLog?.EntityChanges.AddRange(entityChangeList);
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

    /// <summary>
    /// 发布领域事件
    /// </summary>
    /// <param name="changeReport"></param>
    private void PublishEntityEvents(EntityEventReport changeReport)
    {
        // 本地事件
        foreach (var localEvent in changeReport.DomainEvents)
        {
            UnitOfWorkManager.Current?.AddOrReplaceLocalEvent(new UnitOfWorkEventRecord(localEvent.EventData.GetType(), localEvent.EventData, localEvent.EventOrder));
        }

        // 分步式事件
        foreach (var distributedEvent in changeReport.DistributedEvents)
        {
            UnitOfWorkManager.Current?.AddOrReplaceDistributedEvent(new UnitOfWorkEventRecord(distributedEvent.EventData.GetType(), distributedEvent.EventData, distributedEvent.EventOrder));
        }
    }

    /// <summary>
    /// This method will call the DbContext <see cref="SaveChangesAsync(bool, CancellationToken)"/> method directly of EF Core, which doesn't apply concepts of abp.
    /// </summary>
    public virtual Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="initializationContext"></param>
    public virtual void Initialize(EfDbContextInitializationContext initializationContext)
    {
        LazyServiceProvider ??= initializationContext.UnitOfWork.ServiceProvider.GetRequiredService<ILazyServiceProvider>();

        if (initializationContext.UnitOfWork.Options.Timeout.HasValue && Database.IsRelational() && !Database.GetCommandTimeout().HasValue)
            Database.SetCommandTimeout(TimeSpan.FromMilliseconds(initializationContext.UnitOfWork.Options.Timeout.Value));

        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
        ChangeTracker.Tracked += ChangeTracker_Tracked;
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }

    protected virtual void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        FillExtraPropertiesForTrackedEntities(e);
        PublishEventsForTrackedEntity(e.Entry);
    }

    protected virtual void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e) => PublishEventsForTrackedEntity(e.Entry);

    protected virtual void FillExtraPropertiesForTrackedEntities(EntityTrackedEventArgs e)
    {
        var entityType = e.Entry.Metadata.ClrType;
        if (entityType == null)
            return;

        if (e.Entry.Entity is not IHasExtraProperties entity)
            return;

        if (!e.FromQuery)
            return;

        var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
        if (objectExtension == null)
            return;

        foreach (var property in objectExtension.GetProperties())
        {
            if (!property.IsMappedToFieldForEf())
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

    /// <summary>
    /// 保存前属性处理
    /// </summary>
    protected virtual void HandlePropertiesBeforeSave()
    {
        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            HandleExtraPropertiesOnSave(entry);

            if (entry.State.IsIn(EntityState.Modified, EntityState.Deleted))
                UpdateConcurrencyStamp(entry);
        }
    }

    /// <summary>
    /// 创建领域事件
    /// </summary>
    /// <returns></returns>
    protected virtual EntityEventReport CreateEventReport()
    {
        var eventReport = new EntityEventReport();

        // 遍历所有的实体变更事件。
        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            if (entry.Entity is not IGeneratesDomainEvents generatesDomainEventsEntity)
                continue;

            // 本地事件
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

    protected virtual void HandleExtraPropertiesOnSave(EntityEntry entry)
    {
        if (entry.State.IsIn(EntityState.Deleted, EntityState.Unchanged))
            return;

        var entityType = entry.Metadata.ClrType;
        if (entityType == null)
            return;

        if (entry.Entity is not IHasExtraProperties entity)
            return;

        var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
        if (objectExtension == null)
            return;

        var efMappedProperties = ObjectExtensionManager.Instance
            .GetProperties(entityType)
            .Where(p => p.IsMappedToFieldForEf());

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
                SetDeletionAuditProperties(entry);
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
        if (UnitOfWorkManager?.Current?.Items.GetOrDefault(UnitOfWorkItemNames.HardDeletedEntities) is not HashSet<IEntity> hardDeletedEntities)
            return false;

        return hardDeletedEntities.Contains(entry.Entity);
    }

    protected virtual void UpdateConcurrencyStamp(EntityEntry entry)
    {
        if (entry.Entity is not IHasConcurrencyStamp entity)
            return;

        Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected virtual void SetConcurrencyStampIfNull(EntityEntry entry)
    {
        if (entry.Entity is not IHasConcurrencyStamp entity)
            return;

        if (entity.ConcurrencyStamp != null)
            return;

        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected virtual bool TryCancelDeletionForSoftDelete(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDelete)
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

    protected virtual void SetCreationAuditProperties(EntityEntry entry) => AuditPropertySetter?.SetCreationProperties(entry.Entity);

    protected virtual void SetModificationAuditProperties(EntityEntry entry) => AuditPropertySetter?.SetModificationProperties(entry.Entity);

    protected virtual void SetDeletionAuditProperties(EntityEntry entry) => AuditPropertySetter?.SetDeletionProperties(entry.Entity);

    /// <summary>
    /// 配置基础属性
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="modelBuilder"></param>
    /// <param name="mutableEntityType"></param>
    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType) where TEntity : class
    {
        if (mutableEntityType.IsOwned())
            return;

        if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
            return;

        modelBuilder.Entity<TEntity>().ConfigureByConvention();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }

    /// <summary>
    /// 配置全局过滤器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="modelBuilder"></param>
    /// <param name="mutableEntityType"></param>
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

    /// <summary>
    /// 是否过滤
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entityType"></param>
    /// <returns></returns>
    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
    {
        // 多租户
        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            return true;

        // 软件删除
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            return true;

        return false;
    }

    /// <summary>
    /// 创建过滤表达式
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>() where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        // 软删除
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            expression = e => !IsSoftDeleteFilterEnabled || !EF.Property<bool>(e, nameof(ISoftDelete.IsDeleted));

        // 多租户
        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>>? multiTenantFilter = e => !IsMultiTenantFilterEnabled || EF.Property<string>(e, nameof(IMultiTenant.TenantId)) == CurrentTenantId;
            expression = expression == null ? multiTenantFilter : CombineExpressions(expression, multiTenantFilter);
        }

        return expression;
    }

    /// <summary>
    /// 合并表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression1"></param>
    /// <param name="expression2"></param>
    /// <returns></returns>
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

        public override Expression? Visit(Expression? node)
        {
            if (node == _oldValue)
                return _newValue;

            return base.Visit(node);
        }
    }
}
