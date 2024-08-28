using System.Linq.Expressions;

namespace RegApi.Repository.Interfaces
{
    /// <summary>
    /// Base Repository that allows you to interact with database entities
    /// </summary>
    /// <typeparam name="TEntity">The entity which we will interact with</typeparam>
    /// <typeparam name="TEntityId">Type of identifier of TEntity</typeparam>
    public interface IBaseRepository<TEntity, TEntityId> 
        where TEntity : class
        where TEntityId : struct
    {
        /// <summary>
        /// Get all records of table by entity async
        /// </summary>
        /// <param name="includeProperties">Allows to get records with nested objects</param>
        /// <returns>List of entities</returns>
        Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get entity record by identifier async
        /// </summary>
        /// <param name="id">The identifier of the searched entity</param>
        /// <param name="includeProperties">Allows to get record with nested objects</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(TEntityId id, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Add new record to entity table async
        /// </summary>
        /// <param name="entity">New entity record for adding</param>
        /// <returns>The identifier of created record</returns>
        Task<TEntityId> AddAsync(TEntity entity);

        /// <summary>
        /// Update data of record async
        /// </summary>
        /// <param name="entity">Entity record with updated data</param>
        /// <returns>Edited entity record</returns>
        Task<TEntity?> UpdateAsync(TEntity entity);

        /// <summary>
        /// Delete entity record async
        /// </summary>
        /// <param name="id">The identifier of the entity to delete</param>
        /// <returns>Boolean result of deleting</returns>
        Task<bool> DeleteAsync(TEntityId id);
    }
}
