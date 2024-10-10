namespace EmailSender.Models
{
    /// <summary>
    /// Represents the configuration settings required for sending emails.
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// Gets or sets the email address from which emails are sent.
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server address used for sending emails.
        /// </summary>
        public string? SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets the port number used for connecting to the SMTP server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username for authenticating with the SMTP server.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for authenticating with the SMTP server.
        /// </summary>
        public string? Password { get; set; }
    }
}
