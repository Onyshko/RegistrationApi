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
        private IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UnitOfWork(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _userManager = userManager;
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IBaseRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new BaseRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IUserRepository UserRepository() => _userRepository ??= new UserRepository(_userManager, _context);
    }
}
