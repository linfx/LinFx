﻿using LinFx.Domain.Models;

namespace LinFx.Infrastructure.Data.Dapper.Filters.Action
{
    public class NullDapperActionFilterExecuter : IDapperActionFilterExecuter
    {
        public static readonly NullDapperActionFilterExecuter Instance = new NullDapperActionFilterExecuter();

        public void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
        }

        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
        }

        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
        }
    }
}