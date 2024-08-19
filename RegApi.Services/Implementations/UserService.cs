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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IEmailSender emailSender, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        public async Task<IList<string>> RegistrateAsync(UserRegistrationModel userRegistrationModel)
        {
            var user = _mapper.Map<User>(userRegistrationModel);
            var result = await _unitOfWork.UserAccountRepository().RegisterAsync(user, userRegistrationModel.Password!);

            var errors = new List<string>();

            if (!result.Succeeded)
            {
                errors = result.Errors.Select(e => e.Description).ToList();

                return errors;
            }

            var token = await _unitOfWork.UserAccountRepository().GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(userRegistrationModel.ClientUri!, param);

            var message = new Message([user.Email!], "Email.Confirmation Token", callback);

            await _emailSender.SendEmailAsync(message);

            await _unitOfWork.UserAccountRepository().AddToRoleAsync(user, "Visitor");

            await _unitOfWork.SaveChangesAsync();

            return errors;
        }

        public async Task<string> CheckOfUserAsync(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByNameAsync(userAuthenticationModel.Email!);
            if (user is null)
                return "Invalid Request";

            if (!await _unitOfWork.UserAccountRepository().IsEmailConfirmed(user))
                return "Email is not confirmed";

            if (!await _unitOfWork.UserAccountRepository().CheckPassword(user, userAuthenticationModel.Password!))
                return "Invalid Authentication";

            await _unitOfWork.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<string> EmailCheckAsync(string email, string token)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(email);
            if (user is null)
                return "Invalid Email Confirmation Request";

            var confirmatedResult = await _unitOfWork.UserAccountRepository().ConfirmEmailAsync(user, token);
            if (!confirmatedResult.Succeeded)
                return "Invalid Email Confirmation Request";

            await _unitOfWork.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(forgotPasswordModel.Email!);
            if (user is null)
                return false;

            var token = await _unitOfWork.UserAccountRepository().GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", forgotPasswordModel.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordModel.ClientUri!, param);
            
            var message = new Message([user.Email], "Reset password token", callback);

            await _emailSender.SendEmailAsync(message);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IList<string>> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var errorList = new List<string>();
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(resetPasswordModel.Email!);
            if (user is null)
            {
                errorList.Add("Invalid Request");
                return errorList;
            }

            var result = await _unitOfWork.UserAccountRepository().ResetPasswordAsync(user, resetPasswordModel.Token!, resetPasswordModel.Password!);
            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => errorList.Add(error.Description));
            }

            await _unitOfWork.SaveChangesAsync();

            return errorList;
        }
    }
}
