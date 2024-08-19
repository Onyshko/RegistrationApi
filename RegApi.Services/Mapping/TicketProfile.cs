using AutoMapper;
using RegApi.Domain.Entities;
using RegApi.Services.Models;

namespace RegApi.Services.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketModel>().ReverseMap();
        }
    }
}
