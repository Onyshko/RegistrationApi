using MimeKit;
using RegApi.Shared.Models;

namespace EmailSender.Models
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
        /// Populates the list of recipients, subject, and content from an <see cref="EmailSenderModel"/>.
        /// </summary>
        /// <param name="emailSenderModel">Model containing email addresses, subject, and content.</param>
        public Message(EmailSenderModel emailSenderModel)
        {
            To = new List<MailboxAddress>();

            // Adds the email addresses to the recipient list
            To.AddRange(emailSenderModel.Emails.Select(x => new MailboxAddress("email", x)));

            // Sets the subject and content for the email message
            Subject = emailSenderModel.Subject!;
            Content = emailSenderModel.Content!;
        }
    }
}
