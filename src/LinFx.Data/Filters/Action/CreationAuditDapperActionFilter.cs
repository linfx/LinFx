using System;
using LinFx.Domain.Entities;
using LinFx.Domain.Entities.Auditing;
using LinFx.Extensions;
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

            if (!string.IsNullOrEmpty(userId) && entity is ICreationAudited)
            {
                var record = entity as ICreationAudited;
                if (record.CreatorUserId == null)
                {
                    //if (entity is IMayHaveTenant || entity is IMustHaveTenant)
                    //{
                    //    //Sets CreatorUserId only if current user is in same tenant/host with the given entity
                    //    if (entity is IMayHaveTenant && entity.As<IMayHaveTenant>().TenantId == AbpSession.TenantId ||
                    //        entity is IMustHaveTenant && entity.As<IMustHaveTenant>().TenantId == AbpSession.TenantId)
                    //    {
                    //        record.CreatorUserId = userId;
                    //    }
                    //}
                    //else
                    //{
                    //    record.CreatorUserId = userId;
                    //}
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
                record.LastModifierUserId = null;
            }

            //CheckAndSetMustHaveTenantIdProperty(entity);
            //CheckAndSetMayHaveTenantIdProperty(entity);
        }
    }
}
