using Microsoft.EntityFrameworkCore;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace RegApi.Repository.Implementations
{
    /// <summary>
    /// Realisation of IBaseRepository interface
    /// </summary>
    /// <typeparam name="TEntity">Entity for interaction</typeparam>
    /// <typeparam name="TEntityId">Entity identifier type</typeparam>
    public class BaseRepository<TEntity, TEntityId> : IBaseRepository<TEntity, TEntityId>
        where TEntityId : struct
        where TEntity : BaseEntity<TEntityId>
    {
        protected readonly DatabaseContext _context;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all records of table by entity async
        /// </summary>
        /// <param name="includeProperties">Allows to get records with nested objects</param>
        /// <returns>List of entities</returns>
        public async Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get entity record by identifier async
        /// </summary>
        /// <param name="id">The identifier of the searched entity</param>
        /// <param name="includeProperties">Allows to get record with nested objects</param>
        /// <returns>Entity</returns>
        public async Task<TEntity> GetAsync(TEntityId id, params Expression<Func<TEntity, object>>[]? includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Add new record to entity table async
        /// </summary>
        /// <param name="entity">New entity record for adding</param>
        /// <returns>The identifier of created record</returns>
        public async Task<TEntityId> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);

            return entity.Id;
        }

        /// <summary>
        /// Update data of record async
        /// </summary>
        /// <param name="entity">Entity record with updated data</param>
        /// <returns>Edited entity record</returns>
        public async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var existingEntity = await GetAsync(entity.Id);
            if (existingEntity == null)
            {
                return null;
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return existingEntity;
        }

        /// <summary>
        /// Delete entity record async
        /// </summary>
        /// <param name="id">The identifier of the entity to delete</param>
        /// <returns>Boolean result of deleting</returns>
        public async Task<bool> DeleteAsync(TEntityId id)
        {
            var existingEntity = await GetAsync(id);

            if (existingEntity == null)
            {
                return false;
            }

            _context.Entry(existingEntity).State = EntityState.Deleted;
            return true;
        }
    }
}
