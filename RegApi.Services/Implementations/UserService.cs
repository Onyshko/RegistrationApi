using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using RegApi.Domain.Entities;
using RegApi.Repository.Interfaces;
using RegApi.Repository.Models;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

namespace RegApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IEmailSender emailSender, IMapper mapper)
        {
            _userRepo = userRepo;
            _emailSender = emailSender;
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

            var token = await _userRepo.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(userRegistrationModel.ClientUri!, param);

            var message = new Message([user.Email!], "Email.Confirmation Token", callback);

            await _emailSender.SendEmailAsync(message);

            await _userRepo.AddToRoleAsync(user, "Visitor");

            return errors;
        }

        public async Task<string> CheckOfUserAsync(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _userRepo.FindByNameAsync(userAuthenticationModel.Email!);
            if (user is null)
                return "Invalid Request";

            if (!await _userRepo.IsEmailConfirmed(user))
                return "Email is not confirmed";

            if (!await _userRepo.CheckPassword(user, userAuthenticationModel.Password!))
                return "Invalid Authentication";

            return string.Empty;
        }

        public async Task<string> EmailCheckAsync(string email, string token)
        {
            var user = await _userRepo.FindByEmailAsync(email);
            if (user is null)
                return "Invalid Email Confirmation Request";

            var confirmatedResult = await _userRepo.ConfirmEmailAsync(user, token);
            if (!confirmatedResult.Succeeded)
                return "Invalid Email Confirmation Request";

            return string.Empty;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _userRepo.FindByEmailAsync(forgotPasswordModel.Email!);
            if (user is null)
                return false;

            var token = await _userRepo.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", forgotPasswordModel.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordModel.ClientUri!, param);
            
            var message = new Message([user.Email], "Reset password token", callback);

            await _emailSender.SendEmailAsync(message);

            return true;
        }

        public async Task<IList<string>> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var errorList = new List<string>();
            var user = await _userRepo.FindByEmailAsync(resetPasswordModel.Email!);
            if (user is null)
            {
                errorList.Add("Invalid Request");
                return errorList;
            }

            var result = await _userRepo.ResetPasswordAsync(user, resetPasswordModel.Token!, resetPasswordModel.Password!);
            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => errorList.Add(error.Description));
            }

            return errorList;
        }
    }
}
