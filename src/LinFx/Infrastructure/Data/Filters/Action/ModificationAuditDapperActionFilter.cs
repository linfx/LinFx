using LinFx.Domain.Models;
using System;

namespace LinFx.Infrastructure.Data.Filters.Action
{
    public class ModificationAuditDapperActionFilter : DapperActionFilterBase, IDapperActionFilter
    {
        public void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            if (entity is IHasModificationTime)
            {
                entity.As<IHasModificationTime>().LastModificationTime = Clock.Now;
            }

            if (entity is IModificationAudited)
            {
                var record = entity.As<IModificationAudited>();
                //record.LastModifierUserId = Session.UserId;
            }
        }
    }
}
