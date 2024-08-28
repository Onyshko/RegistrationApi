using RegApi.Domain.Entities;

namespace RegApi.Repository.Interfaces
{
    /// <summary>
    /// Provides methods for managing repositories and saving changes in the unit of work context.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Retrieves or creates a repository for the specified entity type and ID type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity for which to get the repository.</typeparam>
        /// <typeparam name="TEntityId">The type of the entity ID, constrained to be a value type.</typeparam>
        /// <returns>An instance of IBaseRepository for the specified entity type.</returns>
        IBaseRepository<TEntity, TEntityId> GetRepository<TEntity, TEntityId>()
            where TEntityId : struct
            where TEntity : BaseEntity<TEntityId>;

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation.</returns>
        IUserAccountRepository UserAccountRepository();

        /// <summary>
        /// Retrieves or creates an instance of the IUserAccountRepository.
        /// </summary>
        /// <returns>An instance of IUserAccountRepository.</returns>
        Task SaveChangesAsync();
    }
}
