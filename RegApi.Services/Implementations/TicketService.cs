using AutoMapper;
using RegApi.Domain.Entities;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;
using RegApi.Shared.Enums;

namespace RegApi.Services.Implementations
{
    /// <summary>
    /// Provides operations for managing tickets, including adding, retrieving, and updating tickets.
    /// </summary>
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for interacting with the repository.</param>
        /// <param name="mapper">The mapper for mapping models to entities.</param>
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new ticket asynchronously and returns its ID.
        /// </summary>
        /// <param name="ticketModel">The ticket details to be added.</param>
        /// <returns>The ID of the newly added ticket.</returns>
        /// <exception cref="NullReferenceException">Thrown when the ticket model is null.</exception>
        public async Task<int> AddTicket(TicketModel ticketModel)
        {
            if (ticketModel is null)
                throw new NullReferenceException();
            var ticketRepository = _unitOfWork.GetRepository<Ticket, int>();
            var entity = _mapper.Map<Ticket>(ticketModel);

            var ticketId = await ticketRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ticketId;
        }

        /// <summary>
        /// Retrieves all tickets asynchronously.
        /// </summary>
        /// <returns>A list of all tickets.</returns>
        public async Task<IList<TicketModel>> GetAllAsync()
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket, int>();
            var ticketModels = _mapper.Map<List<TicketModel>>(await ticketRepository.GetAllAsync());

            await _unitOfWork.SaveChangesAsync();
            return ticketModels;
        }

        /// <summary>
        /// Retrieves all tickets for a specified user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose tickets are to be retrieved.</param>
        /// <returns>A list of tickets associated with the specified user.</returns>
        public async Task<IList<TicketModel>> GetForUserAsync(string userId)
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket, int>();
            var allTicketModels = _mapper.Map<List<TicketModel>>(await ticketRepository.GetAllAsync());
            var ticketsForUser = allTicketModels.Where(ticket => ticket.UserId == userId).ToList();

            await _unitOfWork.SaveChangesAsync();
            return ticketsForUser;
        }

        /// <summary>
        /// Updates the status of a ticket to "Finished" if the ticket belongs to the specified user.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to be updated.</param>
        /// <param name="userId">The ID of the user who owns the ticket.</param>
        /// <exception cref="Exception">Thrown when the ticket is not found or does not belong to the specified user.</exception>
        public async Task UpdateStatus(int ticketId, string userId)
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket, int>();
            var ticket = await ticketRepository.GetAsync(ticketId);

            if (ticket == null || ticket.UserId != userId)
                throw new Exception("Invalid ticket");

            ticket.Status = Status.Finished;
            await ticketRepository.UpdateAsync(ticket);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
