using AutoMapper;
using RegApi.Domain.Entities;
using RegApi.Domain.Enums;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> AddTicket(TicketModel ticketModel)
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket>();
            var entity = _mapper.Map<Ticket>(ticketModel);

            var ticketId = await ticketRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ticketId;
        }

        public async Task<IList<TicketModel>> GetAllAsync()
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket>();
            var ticketModels = _mapper.Map<List<TicketModel>>(await ticketRepository.GetAllAsync());

            await _unitOfWork.SaveChangesAsync();
            return ticketModels;
        }

        public async Task<IList<TicketModel>> GetForUserAsync(string userId)
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket>();
            var allTicketModels = _mapper.Map<List<TicketModel>>(await ticketRepository.GetAllAsync());
            var ticketsForUser = allTicketModels.Where(ticket => ticket.UserId == userId).ToList();

            await _unitOfWork.SaveChangesAsync();
            return ticketsForUser;
        }

        public async Task<string> UpdateStatus(int ticketId, string userId)
        {
            var ticketRepository = _unitOfWork.GetRepository<Ticket>();
            var ticket = await ticketRepository.GetAsync(ticketId);

            if (ticket == null || ticket.UserId != userId)
            {
                return "Invalid ticket";
            }

            ticket.Status = Status.Finished;
            await ticketRepository.UpdateAsync(ticket);

            await _unitOfWork.SaveChangesAsync();
            return "";
        }
    }
}
