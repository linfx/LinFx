using System;
using LinFx.Domain.Entities;
using LinFx.Domain.Uow;
using LinFx.Session;

namespace LinFx.Data.Filters.Action
{
	public interface IDapperActionFilter
	{
		void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
	}

	public abstract class DapperActionFilterBase
	{
		public ISession Session { get; set; }

		public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        protected virtual string GetAuditUserId()
        {
            if (Session != null && !string.IsNullOrEmpty(Session.UserId))
                return Session.UserId;

            return null;
        }

        protected virtual void CheckAndSetId(object entityAsObj)
        {
            var entity = entityAsObj as IEntity;
            if (entity != null && string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString("N");
            }
        }

        //protected virtual int? GetCurrentTenantIdOrNull()
        //{
        //	if(CurrentUnitOfWorkProvider?.Current != null)
        //		return CurrentUnitOfWorkProvider.Current.GetTenantId();

        //	return Session.TenantId;
        //}
    }
}
