using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
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
        /// <summary>
        /// A dictionary to store repositories by their entity types.
        /// </summary>
        private Dictionary<Type, object> _repositories;

        /// <summary>
        /// The repository for managing user account operations.
        /// </summary>
        private IUserAccountRepository _userAccountRepository;

        /// <summary>
        /// The service for handling file operations.
        /// </summary>
        private IFileService _fileService;

        /// <summary>
        /// The service for managing queue operations.
        /// </summary>
        private IQueueService _queueService;

        /// <summary>
        /// The database context for interacting with the database.
        /// </summary>
        private readonly DatabaseContext _context;

        /// <summary>
        /// The user manager for managing user-related operations.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// The Blob container client for interacting with Azure Blob Storage.
        /// </summary>
        private readonly BlobContainerClient _containerClient;

        /// <summary>
        /// The service bus sender for sending messages to Azure Service Bus.
        /// </summary>
        private readonly ServiceBusSender _serviceBusSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified dependencies.
        /// </summary>
        /// <param name="context">The database context for database operations.</param>
        /// <param name="userManager">The user manager for managing users.</param>
        /// <param name="containerClient">The Blob container client for file operations.</param>
        /// <param name="serviceBusSender">The service bus sender for sending messages.</param>
        public UnitOfWork(DatabaseContext context,
                          UserManager<User> userManager,
                          BlobContainerClient containerClient,
                          ServiceBusSender serviceBusSender)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _userManager = userManager;
            _containerClient = containerClient;
            _serviceBusSender = serviceBusSender;
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

        /// <summary>
        /// Provides an instance of <see cref="IFileService"/> for managing files in Azure Blob Storage.
        /// If the service is not initialized, it creates a new instance of <see cref="FileService"/> using the provided Blob container client.
        /// </summary>
        /// <returns>An instance of <see cref="IFileService"/>.</returns>
        public IFileService FileService() => _fileService ??= new FileService(_containerClient);

        /// <summary>
        /// Retrieves or creates an instance of <see cref="IQueueService"/> for managing queue operations.
        /// </summary>
        /// <returns>An instance of <see cref="IQueueService"/>.</returns>
        public IQueueService QueueService() => _queueService ??= new QueueService(_serviceBusSender);
    }
}
