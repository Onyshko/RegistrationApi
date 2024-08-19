using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    public interface ITicketService
    {
        Task<int> AddTicket(TicketModel ticketModel);
        Task<IList<TicketModel>> GetAllAsync();
        Task<IList<TicketModel>> GetForUserAsync(string userId);
        Task<string> UpdateStatus(int ticketId, string userId);
    }
}
