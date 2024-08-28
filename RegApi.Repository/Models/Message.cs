using MimeKit;

namespace RegApi.Repository.Models
{
    /// <summary>
    /// Represents an email message with its recipients, subject, and content.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the list of email addresses to which the message will be sent.
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the content of the email message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="to">A collection of email addresses to send the message to.</param>
        /// <param name="subject">The subject of the email message.</param>
        /// <param name="content">The content of the email message.</param>
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }
    }
}
