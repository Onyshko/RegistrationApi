using System.Text.Json;

namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents the details of an error response, including the status code and message.
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// Gets or sets the HTTP status code associated with the error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message providing additional information about the error.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Serializes the error details to a JSON string representation.
        /// </summary>
        /// <returns>A JSON string representation of the error details.</returns>
        public override string ToString() => JsonSerializer.Serialize(this);
    }

}
