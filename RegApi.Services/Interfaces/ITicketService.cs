using RegApi.Services.Models;

namespace RegApi.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for ticket-related operations such as adding, retrieving, and updating tickets.
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Adds a new ticket asynchronously and returns its ID.
        /// </summary>
        /// <param name="ticketModel">The ticket details to be added.</param>
        /// <returns>The ID of the newly added ticket.</returns>
        /// <exception cref="NullReferenceException">Thrown when the ticket model is null.</exception>
        Task<int> AddTicket(TicketModel ticketModel);

        /// <summary>
        /// Retrieves all tickets asynchronously.
        /// </summary>
        /// <returns>A list of all tickets.</returns>
        Task<IList<TicketModel>> GetAllAsync();

        /// <summary>
        /// Retrieves all tickets for a specified user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose tickets are to be retrieved.</param>
        /// <returns>A list of tickets associated with the specified user.</returns>
        Task<IList<TicketModel>> GetForUserAsync(string userId);

        /// <summary>
        /// Updates the status of a ticket to "Finished" if the ticket belongs to the specified user.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to be updated.</param>
        /// <param name="userId">The ID of the user who owns the ticket.</param>
        /// <exception cref="Exception">Thrown when the ticket is not found or does not belong to the specified user.</exception>
        Task UpdateStatus(int ticketId, string userId);
    }

}
