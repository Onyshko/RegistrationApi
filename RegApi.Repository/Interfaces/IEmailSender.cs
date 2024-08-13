using RegApi.Repository.Models;

namespace RegApi.Repository.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
