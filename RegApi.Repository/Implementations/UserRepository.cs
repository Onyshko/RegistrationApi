using Microsoft.EntityFrameworkCore;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    public class UserRepository : IUSerRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().Include(x => x.Tickets).ToListAsync();
        }

        public async Task<User> GetAsync(string id)
        {
            return (await _context.Users.FirstOrDefaultAsync(x => x.Id == id))!;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingEntity = await GetAsync(user.Id);
            if (existingEntity == null)
            {
                return existingEntity;
            }

            UpdateRelatedEntities(existingEntity, user);
            _context.Entry(existingEntity).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();

            return existingEntity;
        }

        private void UpdateRelatedEntities(User existingEntity, User user)
        {
            var toDeleteAlbums = existingEntity.Tickets.Where(x => !user.Tickets.Any(y => y.Id == x.Id)).ToList();
            foreach (var album in toDeleteAlbums)
            {
                existingEntity.Tickets.Remove(album);
            }

            var toAddAlbums = user.Tickets.Where(x => !existingEntity.Tickets.Any(y => y.Id == x.Id)).ToList();
            foreach (var album in toAddAlbums)
            {
                existingEntity.Tickets.Add(album);
            }
        }
    }
}
