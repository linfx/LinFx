//using LinFx.Infrastructure.Data.Filters.Action;
//using LinFx.Domain.Models;

//namespace LinFx.Extensions.Data.Dapper.Filters.Action
//{
//	public interface IDapperActionFilterExecuter
//	{
//		void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;

//		void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;

//		void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
//	}

//    public class DapperActionFilterExecuter : IDapperActionFilterExecuter
//    {
//        //private readonly IIocResolver _iocResolver;

//        //public DapperActionFilterExecuter(IIocResolver iocResolver)
//        //{
//        //    _iocResolver = iocResolver;
//        //}

//        public void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
//        {
//            //_iocResolver.Resolve<CreationAuditDapperActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
//            new CreationAuditDapperActionFilter().ExecuteFilter<TEntity, TPrimaryKey>(entity);
//        }

//        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
//        {
//            //_iocResolver.Resolve<ModificationAuditDapperActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
//            new ModificationAuditDapperActionFilter().ExecuteFilter<TEntity, TPrimaryKey>(entity);
//        }

//        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
//        {
//            //_iocResolver.Resolve<DeletionAuditDapperActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
//        }
//    }
//}
