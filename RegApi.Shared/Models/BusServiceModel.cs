namespace RegApi.Shared.Models
{
    /// <summary>
    /// Represents the configuration settings for the bus service.
    /// </summary>
    public class BusServiceModel
    {
        /// <summary>
        /// Gets or sets the connection string for the bus service.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the email queue used in the bus service.
        /// </summary>
        public string? EmailQueue { get; set; }
    }
}