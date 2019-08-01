//using System;
//using LinFx.Domain.Models;

//namespace LinFx.Extensions.Data.Filters.Action
//{
//    public class CreationAuditDapperActionFilter : DapperActionFilterBase, IDapperActionFilter
//    {
//        //private readonly IMultiTenancyConfig _multiTenancyConfig;

//        //public CreationAuditDapperActionFilter(IMultiTenancyConfig multiTenancyConfig)
//        //{
//        //    _multiTenancyConfig = multiTenancyConfig;
//        //}

//        public void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
//        {
//            var userId = GetAuditUserId();
//            CheckAndSetId(entity);
//            if (!(entity is IHasCreationTime entityWithCreationTime))
//                return;

//            if (entityWithCreationTime.CreationTime == default)
//            {
//                entityWithCreationTime.CreationTime = Clock.Now;
//            }

//            if (userId > 0 && entity is ICreationAudited)
//            {
//                var record = entity as ICreationAudited;
//                if (record.CreatorUserId == 0)
//                {
//                    record.CreatorUserId = userId;
//                }
//            }

//            if (entity is IHasModificationTime)
//            {
//                entity.As<IHasModificationTime>().LastModificationTime = null;
//            }

//            if (entity is IModificationAudited)
//            {
//                var record = entity.As<IModificationAudited>();
//                record.LastModifierUserId = 0;
//            }

//            //CheckAndSetMustHaveTenantIdProperty(entity);
//            //CheckAndSetMayHaveTenantIdProperty(entity);
//        }
//    }
//}
