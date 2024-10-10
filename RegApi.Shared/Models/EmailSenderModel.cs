namespace RegApi.Shared.Models
{
    /// <summary>
    /// Represents the model used for sending email messages.
    /// </summary>
    public class EmailSenderModel
    {
        /// <summary>
        /// Gets or sets the list of email addresses to which the message will be sent.
        /// </summary>
        public IEnumerable<string>? Emails { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email message.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the content of the email message.
        /// </summary>
        public string? Content { get; set; }
    }
}
