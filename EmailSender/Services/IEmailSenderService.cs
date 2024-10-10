using EmailSender.Models;

namespace EmailSender.Services
{
    /// <summary>
    /// Defines methods for sending emails.
    /// </summary>
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sends an email using the provided message details.
        /// </summary>
        /// <param name="message">The message containing email details to be sent.</param>
        void SendEmail(Message message);

        /// <summary>
        /// Sends an email asynchronously using the provided message details.
        /// </summary>
        /// <param name="message">The message containing email details to be sent.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        Task SendEmailAsync(Message message);
    }
}
