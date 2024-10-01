namespace RegApi.Repository.Models.BlobModels
{
    /// <summary>
    /// Represents the response model for blob operations, including upload and retrieval.
    /// </summary>
    public class BlobResponseModel
    {
        /// <summary>
        /// Gets or sets the status message of the blob operation.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an error occurred during the blob operation.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets the details of the blob associated with the response.
        /// </summary>
        public BlobModel Blob { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobResponseModel"/> class.
        /// </summary>
        public BlobResponseModel()
        {
            Blob = new BlobModel();
        }
    }

}
