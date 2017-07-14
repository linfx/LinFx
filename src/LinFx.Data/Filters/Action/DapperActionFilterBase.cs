using LinFx.Domain.Entities;
using LinFx.Domain.Uow;
using LinFx.Session;

namespace LinFx.Data.Dapper.Filters.Action
{
	public interface IDapperActionFilter
	{
		void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
	}

	public abstract class DapperActionFilterBase
	{
		protected DapperActionFilterBase()
		{
			//Session = NullSession.Instance;
			//GuidGenerator = SequentialGuidGenerator.Instance;
		}

		public ISession Session { get; set; }

		public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

		//public IGuidGenerator GuidGenerator { get; set; }

		//protected virtual long? GetAuditUserId()
		//{
		//	if(Session.UserId.HasValue && CurrentUnitOfWorkProvider?.Current != null)
		//		return Session.UserId;

		//	return null;
		//}

		//protected virtual void CheckAndSetId(object entityAsObj)
		//{
		//	var entity = entityAsObj as IEntity<Guid>;
		//	if(entity != null && entity.Id == Guid.Empty)
		//	{
		//		Type entityType = ObjectContext.GetObjectType(entityAsObj.GetType());
		//		PropertyInfo idProperty = entityType.GetProperty("Id");
		//		var dbGeneratedAttr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
		//		if(dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
		//		{
		//			entity.Id = GuidGenerator.Create();
		//		}
		//	}
		//}

		//protected virtual int? GetCurrentTenantIdOrNull()
		//{
		//	if(CurrentUnitOfWorkProvider?.Current != null)
		//		return CurrentUnitOfWorkProvider.Current.GetTenantId();

		//	return Session.TenantId;
		//}
	}
}
