using Azure.Messaging.ServiceBus;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using RegApi.Repository.Implementations;
using RegApi.Repository.Interfaces;
using RegApi.Repository.Models.BlobModels;
using RegApi.Shared.Models;

namespace RegApi.Repository.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering utility services in the dependency injection container.
    /// </summary>
    public static class UtilityRegistration
    {
        /// <summary>
        /// Registers utility services and Azure Blob Storage client in the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the utility services will be added.</param>
        public static void AddUtilityRegistration(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IQueueService, QueueService>();

            services.AddScoped(x =>
            {
                var blobConfig = x.GetRequiredService<BlobConfigurationModel>();
                var credential = new StorageSharedKeyCredential(blobConfig.StorageName, blobConfig.Key);
                var blobUri = new Uri($"https://{blobConfig.StorageName}.blob.core.windows.net");
                var blobServiceClient = new BlobServiceClient(blobUri, credential);

                return blobServiceClient.GetBlobContainerClient("avatars");
            });

            services.AddScoped(provider =>
            {
                var busService = provider.GetRequiredService<BusServiceModel>();
                var serviceBusClient = new ServiceBusClient(busService.ConnectionString);
                
                return serviceBusClient.CreateSender("messageforemail");
            });
        }
    }

}
