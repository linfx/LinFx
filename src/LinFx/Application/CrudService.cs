using LinFx.Application.Models;
using LinFx.Domain.Models;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.ObjectMapping;
using LinFx.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DbContext = LinFx.Extensions.EntityFrameworkCore.DbContext;

namespace LinFx.Application
{
    public abstract class CrudService<TEntity> 
        : CrudService<TEntity, TEntity, TEntity, string, PagedAndSortedResultRequest, TEntity, TEntity>
        where TEntity : class, IEntity<string>
    {
        protected CrudService(DbContext context) : base(context) { }
    }

    public abstract class CrudService<TEntity, TOutput, TListOutput, TKey, TListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TKey>
           where TListInput : IPagedAndSortedResultRequest
    {
        protected DbContext _context;

        protected CrudService(DbContext context)
        {
            _context = context;
        }

        public virtual async Task<PagedResult<TListOutput>> GetListAsync(TListInput input)
        {
            var query = CreateFilteredQuery(input);

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await query.ToListAsync();
            return new PagedResult<TListOutput>(input, totalCount, entities.Select(MapToListOutput).ToList());
        }

        public virtual async Task<TOutput> GetAsync(TKey id)
        {
            var entity = await GetEntityByIdAsync(id);
            return MapToOutput(entity);
        }

        public virtual async Task<TOutput> CreateAsync(TCreateInput input)
        {
            var entity = MapToEntity(input);
            TryToSetTenantId(entity);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return MapToOutput(entity);
        }

        public virtual async Task<TOutput> UpdateAsync(TKey id, TUpdateInput input)
        {
            var entity = await GetEntityByIdAsync(id);
            MapToEntity(input, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return MapToOutput(entity);
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await GetEntityByIdAsync(id);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        protected virtual Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            return _context.FindAsync<TEntity>(id);
        }



        protected virtual TListOutput MapToListOutput(TEntity entity)
        {
            return ObjectMapper.Map<TEntity, TListOutput>(entity);
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
            return _context.Set<TEntity>();
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

        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
            var entity = ObjectMapper.Map<TCreateInput, TEntity>(createInput);
            SetId(entity);
            return entity;
        }

        protected virtual void SetId(TEntity entity)
        {
            if (entity is IEntity<string> entityWithStringId)
            {
                if (string.IsNullOrWhiteSpace(entityWithStringId.Id))
                    entityWithStringId.Id = IDUtils.NewId().ToString();
            }
            return;
        }

        protected virtual TOutput MapToOutput(TEntity entity)
        {
            return ObjectMapper.Map<TEntity, TOutput>(entity);
        }

        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            if (updateInput is IEntity<TKey> entityDto)
            {
                entityDto.Id = entity.Id;
            }
            ObjectMapper.Map(updateInput, entity);
        }

        protected virtual void TryToSetTenantId(TEntity entity)
        {
            if (entity is IMultiTenant && HasTenantIdProperty(entity))
            {
                //var tenantId = CurrentTenant.Id;

                //if (!tenantId.HasValue)
                //{
                //    return;
                //}

                //var propertyInfo = entity.GetType().GetProperty(nameof(IMultiTenant.TenantId));

                //if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                //{
                //    propertyInfo.SetValue(entity, tenantId, null);
                //}
            }
        }

        protected virtual bool HasTenantIdProperty(TEntity entity)
        {
            return entity.GetType().GetProperty(nameof(IMultiTenant.TenantId)) != null;
        }
    }
}
