using Microsoft.AspNetCore.Http;
using RegApi.Repository.Models.BlobModels;

namespace RegApi.Repository.Interfaces
{
    /// <summary>
    /// Defines operations for managing file storage in a blob container.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Retrieves all files stored in the blob container.
        /// </summary>
        /// <returns>A <see cref="List{BlobModel}"/> containing details of all stored blobs.</returns>
        Task<List<BlobModel>> GetAllAsync();

        /// <summary>
        /// Uploads a file to the blob container.
        /// </summary>
        /// <param name="blob">The file to be uploaded as an <see cref="IFormFile"/>.</param>
        /// <returns>A <see cref="BlobResponseModel"/> containing the status of the upload operation and blob details.</returns>
        Task<BlobResponseModel> UploadAsync(IFormFile blob);
    }
}
