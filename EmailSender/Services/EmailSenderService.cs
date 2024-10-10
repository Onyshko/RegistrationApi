using MailKit.Net.Smtp;
using MimeKit;
using EmailSender.Models;

namespace EmailSender.Services
{
    /// <summary>
    /// Provides functionality to send emails using SMTP.
    /// </summary>
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSenderService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        /// <summary>
        /// Sends an email using the provided message details.
        /// </summary>
        /// <param name="message">The message containing email details to be sent.</param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        /// <summary>
        /// Sends an email asynchronously using the provided message details.
        /// </summary>
        /// <param name="message">The message containing email details to be sent.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        /// <summary>
        /// Creates a MimeMessage object based on the provided message details.
        /// </summary>
        /// <param name="message">The message containing details for creating the email.</param>
        /// <returns>A MimeMessage object configured with the message details.</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        /// <summary>
        /// Sends the provided MimeMessage using an SMTP client.
        /// </summary>
        /// <param name="mailMessage">The MimeMessage to be sent.</param>
        /// <exception cref="Exception">Throws an exception if the sending fails.</exception>
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Asynchronously sends the provided MimeMessage using an SMTP client.
        /// </summary>
        /// <param name="mailMessage">The MimeMessage to be sent.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        /// <exception cref="Exception">Throws an exception if the sending fails.</exception>
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
