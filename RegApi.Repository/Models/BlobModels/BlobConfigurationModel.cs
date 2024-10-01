namespace RegApi.Repository.Models.BlobModels
{
    /// <summary>
    /// Represents the configuration settings for Azure Blob Storage.
    /// </summary>
    public class BlobConfigurationModel
    {
        /// <summary>
        /// Gets or sets the name of the blob storage account.
        /// </summary>
        public string? StorageName { get; set; }

        /// <summary>
        /// Gets or sets the access key for the blob storage account.
        /// </summary>
        public string? Key { get; set; }
    }
}
