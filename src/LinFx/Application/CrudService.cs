using LinFx.Application.Models;
using LinFx.Domain.Models;
using LinFx.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DbContext = LinFx.Data.DbContext;

namespace LinFx.Application
{
    public abstract class CrudService<TEntity>
        : CrudService<TEntity, TEntity, TEntity, string, PagedAndSortedResultRequest, TEntity, TEntity>
            where TEntity : class, IEntity<string>
    {
        protected CrudService(ServiceContext context, DbContext db)
            : base(context, db) { }
    }

    public abstract class CrudService<TEntity, TEntityInput>
        : CrudService<TEntity, TEntity, TEntity, string, PagedAndSortedResultRequest, TEntityInput, TEntityInput>
            where TEntity : class, IEntity<string>
            where TEntityInput : class
    {
        protected CrudService(ServiceContext context, DbContext db)
            : base(context, db) { }
    }

    public abstract class CrudService<TEntity, TOutput, TListOutput, TKey, TListInput, TCreateInput, TUpdateInput>
        : ApplicationService
           where TEntity : class, IEntity<TKey>
           where TOutput : class
           where TListOutput : class
           where TListInput : class, IPagedAndSortedResultRequest
           where TUpdateInput : class
    {
        protected readonly DbContext _db;

        protected CrudService(ServiceContext context, DbContext db) : base(context)
        {
            _db = db;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<TListOutput>> GetListAsync(TListInput input)
        {
            var query = CreateFilteredQuery(input);

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await query.AsNoTracking().ToListAsync();
            return new PagedResult<TListOutput>(input, totalCount, entities.Select(MapToListOutput).ToList());
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TOutput> GetAsync(TKey id)
        {
            var entity = await FindEntityByIdAsync(id);
            return MapToOutput(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TOutput> CreateAsync(TCreateInput input)
        {
            var entity = MapToEntity(input);
            TryToSetTenantId(entity);
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return MapToOutput(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TOutput> UpdateAsync(TKey id, TUpdateInput input)
        {
            var entity = await FindEntityByIdAsync(id);
            MapToEntity(input, entity);
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return MapToOutput(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await FindEntityByIdAsync(id);
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        protected virtual async Task<TEntity> FindEntityByIdAsync(TKey id)
        {
            var entity = await _db.FindAsync<TEntity>(id);
            if (entity == null)
                throw new UserFriendlyException($"对象[{id}]不存在");

            return entity;
        }

        /// <summary>
        /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
        /// It should filter query if needed, but should not do sorting or paging.
        /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
        /// methods.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TListInput input)
        {
            return _db.Set<TEntity>();
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TListInput input)
        {
            //Try to sort query if available
            if (input is ISortedResultRequest sortInput)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    //return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TListInput input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.Limit);
            }

            //No paging
            return query;
        }

        protected virtual TEntity MapToEntity<TInput>(TInput input)
        {
            var entity = input.MapTo<TEntity>();
            SetId(entity);
            return entity;
        }

        protected virtual void MapToEntity(TUpdateInput input, TEntity entity)
        {
            var id = entity.Id;
            input.MapTo(entity);
            entity.Id = id;
        }

        protected virtual TOutput MapToOutput(TEntity entity)
        {
            return entity.MapTo<TOutput>();
        }

        protected virtual TListOutput MapToListOutput(TEntity entity)
        {
            return entity.MapTo<TListOutput>();
        }
    }
}
