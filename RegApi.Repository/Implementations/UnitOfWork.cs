using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private Dictionary<Type, object> _repositories;
        private IUserAccountRepository _userAccountRepository;
        private IUSerRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UnitOfWork(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _userManager = userManager;
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IBaseRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new BaseRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IUserAccountRepository UserAccountRepository() => _userAccountRepository ??= new UserAccountRepository(_userManager);
        public IUSerRepository UserRepository() => _userRepository ??= new UserRepository(_context);
    }
}
