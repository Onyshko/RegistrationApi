using Microsoft.EntityFrameworkCore;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> 
        where T : BaseEntity
    {
        protected readonly DatabaseContext _context;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return (await _context.Set<T>().FindAsync(id))!;
        }

        public async Task<int> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity.Id;
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            var existingEntity = await GetAsync(entity.Id);

            if (existingEntity == null)
            {
                return existingEntity;
            }

            UpdateRelatedEntities(existingEntity, entity);
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            return existingEntity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingEntity = await GetAsync(id);

            if (existingEntity == null)
            {
                return false;
            }

            _context.Entry(existingEntity).State = EntityState.Deleted;
            return true;
        }

        protected virtual void UpdateRelatedEntities(T existingEntity, T entity)
        {
        }
    }
}
