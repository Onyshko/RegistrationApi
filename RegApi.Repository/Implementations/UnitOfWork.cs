using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Context;
using RegApi.Repository.Interfaces;
using RegApi.Repository.Models;
using RegApi.Repository.Models.BlobModels;

namespace RegApi.Repository.Implementations
{
    /// <summary>
    /// Implements the Unit of Work pattern to manage repositories and handle changes in the database context.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<Type, object> _repositories;
        private IUserAccountRepository _userAccountRepository;
        private IEmailSender _emailSender;
        private IFileService _fileService;
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly EmailConfiguration _emailConfig;
        private readonly BlobContainerClient _containerClient;
        private readonly BlobConfigurationModel _blobConfigurationModel;

        public UnitOfWork(DatabaseContext context,
                          UserManager<User> userManager,
                          EmailConfiguration emailConfig,
                          BlobContainerClient containerClient,
                          BlobConfigurationModel blobConfigurationModel)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _userManager = userManager;
            _emailConfig = emailConfig;
            _blobConfigurationModel = blobConfigurationModel;
            _containerClient = containerClient;
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
        /// Provides an instance of <see cref="IEmailSender"/> service. If the service is not initialized, it creates a new instance of <see cref="EmailSender"/> with the specified configuration.
        /// </summary>
        /// <returns>An instance of <see cref="IEmailSender"/>.</returns>
        public IEmailSender EmailSender() => _emailSender ??= new EmailSender(_emailConfig);

        /// <summary>
        /// Provides an instance of <see cref="IFileService"/> for managing files in Azure Blob Storage. If the service is not initialized, it creates a new instance of <see cref="FileService"/> using the provided Blob container client.
        /// </summary>
        /// <returns>An instance of <see cref="IFileService"/>.</returns>
        public IFileService FileService() => _fileService ??= new FileService(_containerClient);
    }
}
