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
            var existingEntity = await GetAsync(entity.Id, includeProperties: GetIncludeProperties());
            if (existingEntity == null)
            {
                return null;
            }

            UpdateRelatedEntities(existingEntity, entity);
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

        /// <summary>
        /// Help update related entities
        /// </summary>
        /// <param name="existingEntity">Entity from database</param>
        /// <param name="entity">Updated entity</param>
        private void UpdateRelatedEntities(TEntity existingEntity, TEntity entity)
        {
            var navigationProperties = _context.Entry(existingEntity).Navigations.Where(n => n.Metadata.IsCollection);

            foreach (var navigation in navigationProperties)
            {
                var existingCollection = (IEnumerable<object>)navigation.CurrentValue!;
                var updatedCollection = (IEnumerable<object>)navigation.Metadata.PropertyInfo?.GetValue(entity)!;

                var toDelete = existingCollection.Where(x => !updatedCollection.Any(y => GetPrimaryKeyValue(y).Equals(GetPrimaryKeyValue(x)))).ToList();
                var toAdd = updatedCollection.Where(x => !existingCollection.Any(y => GetPrimaryKeyValue(y).Equals(GetPrimaryKeyValue(x)))).ToList();

                foreach (var item in toDelete)
                {
                    navigation.Metadata.PropertyInfo?.GetValue(existingEntity)?.GetType().GetMethod("Remove")?.Invoke(navigation.CurrentValue, new[] { item });
                }

                foreach (var item in toAdd)
                {
                    navigation.Metadata.PropertyInfo?.GetValue(existingEntity)?.GetType().GetMethod("Add")?.Invoke(navigation.CurrentValue, new[] { item });
                }
            }
        }

        /// <summary>
        /// Retrieves the value of the primary key for a given entity.
        /// </summary>
        /// <param name="entity">The entity from which to extract the primary key value.</param>
        /// <returns>The primary key value of the entity, or null if the primary key cannot be found.</returns>
        private object GetPrimaryKeyValue(object entity)
        {
            var key = _context.Model.FindEntityType(entity.GetType())?.FindPrimaryKey();
            return key?.Properties.Select(p => entity.GetType().GetProperty(p.Name)?.GetValue(entity)).FirstOrDefault()!;
        }

        /// <summary>
        /// Identifies and returns an array of expressions representing the collection navigation properties
        /// of the TEntity type for inclusion in queries.
        /// </summary>
        /// <returns>An array of expressions representing the collection navigation properties, 
        /// or an empty array if none are found.</returns>
        private Expression<Func<TEntity, object>>[] GetIncludeProperties()
        {
            var navigationProperties = _context.Model.FindEntityType(typeof(TEntity))?.GetNavigations()
                .Where(n => n.IsCollection)
                .Select(n => n.PropertyInfo)
                .ToArray();

            if (navigationProperties != null)
            {
                return navigationProperties.Select(np => (Expression<Func<TEntity, object>>)(entity => np.GetValue(entity)!)).ToArray();
            }

            return Array.Empty<Expression<Func<TEntity, object>>>();
        }
    }
}
