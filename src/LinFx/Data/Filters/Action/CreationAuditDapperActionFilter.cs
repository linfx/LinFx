using System;
using LinFx.Domain.Entities;
using LinFx.Domain.Entities.Auditing;
using LinFx.Timing;

namespace LinFx.Data.Filters.Action
{
    public class CreationAuditDapperActionFilter : DapperActionFilterBase, IDapperActionFilter
    {
        //private readonly IMultiTenancyConfig _multiTenancyConfig;

        //public CreationAuditDapperActionFilter(IMultiTenancyConfig multiTenancyConfig)
        //{
        //    _multiTenancyConfig = multiTenancyConfig;
        //}

        public void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            var userId = GetAuditUserId();
            CheckAndSetId(entity);
            var entityWithCreationTime = entity as IHasCreationTime;
            if (entityWithCreationTime == null)
                return;

            if (entityWithCreationTime.CreationTime == default(DateTime))
            {
                entityWithCreationTime.CreationTime = Clock.Now;
            }

            if (userId > 0 && entity is ICreationAudited)
            {
                var record = entity as ICreationAudited;
                if (record.CreatorUserId == 0)
                {
                    record.CreatorUserId = userId;
                }
            }

            if (entity is IHasModificationTime)
            {
                entity.As<IHasModificationTime>().LastModificationTime = null;
            }

            if (entity is IModificationAudited)
            {
                var record = entity.As<IModificationAudited>();
                record.LastModifierUserId = 0;
            }

            //CheckAndSetMustHaveTenantIdProperty(entity);
            //CheckAndSetMayHaveTenantIdProperty(entity);
        }
    }
}
