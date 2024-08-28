using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    /// <summary>
    /// Implements the Unit of Work pattern to manage repositories and handle changes in the database context.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private Dictionary<Type, object> _repositories;
        private IUserAccountRepository _userAccountRepository;
        private readonly UserManager<User> _userManager;

        public UnitOfWork(DatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves or creates a repository for the specified entity type and ID type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity for which to get the repository.</typeparam>
        /// <typeparam name="TEntityId">The type of the entity ID, constrained to be a value type.</typeparam>
        /// <returns>An instance of IBaseRepository for the specified entity type.</returns>
        public IBaseRepository<TEntity, TEntityId> GetRepository<TEntity, TEntityId>()
            where TEntityId : struct
            where TEntity : BaseEntity<TEntityId>
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IBaseRepository<TEntity, TEntityId>)_repositories[typeof(TEntity)];
            }

            var repository = new BaseRepository<TEntity, TEntityId>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes of the context and releases all resources asynchronously.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Retrieves or creates an instance of the IUserAccountRepository.
        /// </summary>
        /// <returns>An instance of IUserAccountRepository.</returns>
        public IUserAccountRepository UserAccountRepository() => _userAccountRepository ??= new UserAccountRepository(_userManager);
    }
}
