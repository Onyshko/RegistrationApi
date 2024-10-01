namespace RegApi.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for generating Shared Access Signature (SAS) tokens for Azure Blob Storage.
    /// </summary>
    public interface ISasService
    {
        /// <summary>
        /// Asynchronously creates a SAS token for accessing a user's avatar stored in Azure Blob Storage.
        /// </summary>
        /// <param name="email">The email of the user for whom the SAS token is being generated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated SAS token as a string.</returns>
        Task<string> CreateToken(string email);
    }

}
