namespace RegApi.Repository.Models.BlobModels
{
    /// <summary>
    /// Represents a model for a blob stored in Azure Blob Storage.
    /// </summary>
    public class BlobModel
    {
        /// <summary>
        /// Gets or sets the URI of the blob.
        /// </summary>
        public string? Uri { get; set; }

        /// <summary>
        /// Gets or sets the name of the blob.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the content type of the blob.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content stream of the blob.
        /// </summary>
        public Stream? Content { get; set; }
    }

}
