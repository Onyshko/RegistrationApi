using AutoMapper;
using RegApi.Domain.Entities;
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

        public async Task<IList<string>> RegistrateAsync(UserRegistrationModel userRegistrationModel)
        {
            var user = _mapper.Map<User>(userRegistrationModel);
            var result = await _userRepo.RegisterAsync(user, userRegistrationModel.Password!);

            var errors = new List<string>();

            if (!result.Succeeded)
            {
                errors = result.Errors.Select(e => e.Description).ToList();

                return errors;
            }

            await _userRepo.AddToRoleAsync(user, "Visitor");

            return errors;
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
