using LinFx.Domain.Entities;
using LinFx.Domain.Entities.Auditing;
using LinFx.Extensions;
using LinFx.Timing;

namespace LinFx.Data.Filters.Action
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
