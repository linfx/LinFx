using System;
using LinFx.Domain.Models;

namespace LinFx.Extensions.Data.Filters.Action
{
    public interface IDapperActionFilter
	{
		void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
	}

	public abstract class DapperActionFilterBase
	{
        //public ILinFxSession Session { get; set; } = new ClaimsLinFxSession(HttpContext.PrincipalAccessor);

        //public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        protected virtual long GetAuditUserId()
        {
            //return Session.UserId;
            return 0;
        }

        protected virtual void CheckAndSetId(object entityAsObj)
        {
            if (entityAsObj is IEntity<string> entity && string.IsNullOrEmpty(entity.Id))
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
