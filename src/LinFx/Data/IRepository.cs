using LinFx.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Data
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : IEntity<TPrimaryKey>
    {
		/// <summary>
		/// Insert entity.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task InsertAsync(TEntity item);
		/// <summary>
		/// Update entity.
		/// </summary>
		/// <param name="item"></param>
		Task UpdateAsync(TEntity item);
		/// <summary>
		/// Delete entity.
		/// </summary>
		/// <param name="id"></param>
		Task DeleteAsync(TPrimaryKey id);
		/// <summary>
		/// Delete an entity.
		/// </summary>
		/// <param name="item"></param>
		Task DeleteAsync(TEntity item);
		/// <summary>
		/// Gets an entity with given primary key.
		/// </summary>
		/// <param name="id">Primary key of the entity to get</param>
		/// <returns>Entity</returns>
		TEntity Get(TPrimaryKey id);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();
    }

    /// <summary>
    //     A shortcut of IRepository`2 for most used primary key
    //     type (System.String).
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, string> where TEntity : IEntity<string>
    {
    }
}
