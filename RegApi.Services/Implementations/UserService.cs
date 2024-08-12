using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RegApi.Domain.Entities;
using RegApi.Repository.Handlers;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegistrateAsync(UserRegistrationModel userRegistrationModel)
        {
            var user = _mapper.Map<User>(userRegistrationModel);
            return await _userRepo.RegisterAsync(user, userRegistrationModel.Password!);
        }

        public async Task<bool> CheckPassword(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _userRepo.FindByNameAsync(userAuthenticationModel.Email!);
            if (user == null || !await _userRepo.CheckPassword(user, userAuthenticationModel.Password!))
                return false;

            return true;
        }
    }
}
