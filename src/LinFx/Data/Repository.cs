﻿using LinFx.Domain.Entities;
using LinFx.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinFx.Data
{
    /// <summary>
    /// 泛型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public Repository(DbContext context)
        {
            Context = context;
        }

        protected DbContext Context { get; }

        protected virtual DbSet<TEntity> DbSet
        {
            get { return Context.Set<TEntity>(); }
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
                DbSet.Add(entity);
        }

        public void AddRange(IList<TEntity> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                DbSet.AddRange(entities);
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public IQueryable<TEntity> Query() => DbSet;

        public void Remove(TEntity entity)
        {
            if (entity != null)
                Context.Remove(entity);
        }
    }
}
