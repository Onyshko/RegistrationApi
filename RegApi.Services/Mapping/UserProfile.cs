using AutoMapper;
using RegApi.Domain.Entities;
using RegApi.Services.Models;

namespace RegApi.Services.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationModel, User>()
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email));
        }

    }
}
