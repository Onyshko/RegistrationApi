using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using RegApi.Repository.Interfaces;
using RegApi.Repository.Models.BlobModels;

namespace RegApi.Repository.Implementations
{
    public class FileService : IFileService
    {
        private readonly BlobContainerClient _containerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileService"/> class with the specified BlobContainerClient.
        /// </summary>
        /// <param name="containerClient">The Blob container client used for accessing Azure Blob Storage.</param>
        public FileService(BlobContainerClient containerClient)
        {
            _containerClient = containerClient;
        }

        /// <summary>
        /// Retrieves all blobs from the Azure Blob Storage container.
        /// </summary>
        /// <returns>A list of <see cref="BlobModel"/> objects, each representing a file in the container, including its URI, name, and content type.</returns>
        public async Task<List<BlobModel>> GetAllAsync()
        {
            var files = new List<BlobModel>();

            await foreach (var file in _containerClient.GetBlobsAsync())
            {
                string uri = _containerClient.Uri.ToString();
                var name = file.Name;

                files.Add(new BlobModel
                {
                    Uri = $"{uri}{name}",
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }
            
            return files;
        }

        /// <summary>
        /// Uploads the specified file to Azure Blob Storage.
        /// </summary>
        /// <param name="blob">The file to be uploaded.</param>
        /// <returns>A <see cref="BlobResponseModel"/> object containing the status of the upload and the file details such as URI and name.</returns>
        public async Task<BlobResponseModel> UploadAsync(IFormFile blob)
        {
            var client = _containerClient.GetBlobClient(blob.FileName);

            await using (Stream ? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            var response = new BlobResponseModel()
            {
                Status = $"File {blob.FileName} Uploaded Successfully",
                Error = false,
                Blob = new BlobModel
                {
                    Uri = client.Uri.AbsoluteUri,
                    Name = client.Name
                }
            };

            return response;
        }
    }
}
