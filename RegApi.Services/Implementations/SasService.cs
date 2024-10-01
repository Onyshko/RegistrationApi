using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;

namespace RegApi.Services.Implementations
{
    /// <summary>
    /// Provides methods for creating Shared Access Signature (SAS) tokens for accessing Azure Blob Storage.
    /// </summary>
    public class SasService : ISasService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SasService"/> class.
        /// </summary>
        /// <param name="containerClient">The <see cref="BlobContainerClient"/> used to interact with Azure Blob Storage.</param>
        /// <param name="unitOfWork">The <see cref="IUnitOfWork"/> for accessing user account information.</param>
        public SasService(BlobContainerClient containerClient, IUnitOfWork unitOfWork)
        {
            _containerClient = containerClient;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a SAS token for the user's avatar stored in Azure Blob Storage.
        /// </summary>
        /// <param name="email">The email of the user whose avatar URI is being retrieved.</param>
        /// <returns>A task representing the asynchronous operation, containing the SAS token as a string.</returns>
        public async Task<string> CreateToken(string email)
        {
            var fileUri = (await _unitOfWork.UserAccountRepository().FindByEmailAsync(email))!.AvatarUri;

            var blobUri = new Uri(fileUri!);
            var fileName = Path.GetFileName(blobUri.LocalPath);

            var blobClient = _containerClient.GetBlobClient(fileName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobName = fileName,
                BlobContainerName = "avatars",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
    }
}
