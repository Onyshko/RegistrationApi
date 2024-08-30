using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using RegApi.Domain.Entities;
using RegApi.Repository.Interfaces;
using RegApi.Repository.Models;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;
using RegApi.Shared.Extensions.Exceptions;

namespace RegApi.Services.Implementations
{
    /// <summary>
    /// Provides user-related operations such as registration, authentication, and password management.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for interacting with the repository.</param>
        /// <param name="emailSender">The email sender for sending confirmation and reset emails.</param>
        /// <param name="mapper">The mapper for mapping models to entities.</param>
        public UserService(IUnitOfWork unitOfWork, IEmailSender emailSender, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user asynchronously by creating an account and sending a confirmation email.
        /// </summary>
        /// <param name="userRegistrationModel">The user registration details including email, password, and client URI.</param>
        /// <exception cref="NullReferenceException">Thrown when the user registration model is null.</exception>
        /// <exception cref="IdentityException">Thrown when the registration process fails due to identity errors.</exception>
        public async Task RegistrateAsync(UserRegistrationModel userRegistrationModel)
        {
            if (userRegistrationModel is null)
                throw new NullReferenceException();

            var user = _mapper.Map<User>(userRegistrationModel);
            var result = await _unitOfWork.UserAccountRepository().RegisterAsync(user, userRegistrationModel.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                throw new IdentityException(errors);
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
        }

        /// <summary>
        /// Checks if the user exists, has confirmed their email, and if the provided password is correct.
        /// </summary>
        /// <param name="userAuthenticationModel">The user authentication details including email and password.</param>
        /// <exception cref="NullReferenceException">Thrown when the user does not exist.</exception>
        /// <exception cref="AuthenticateException">Thrown when the email is not confirmed or authentication fails due to an invalid password.</exception>
        public async Task CheckOfUserAsync(UserAuthenticationModel userAuthenticationModel)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByNameAsync(userAuthenticationModel.Email!);
            if (user is null)
                throw new NullReferenceException();

            if (!await _unitOfWork.UserAccountRepository().IsEmailConfirmed(user))
                throw new AuthenticateException("Email is not confirmed");

            if (!await _unitOfWork.UserAccountRepository().CheckPassword(user, userAuthenticationModel.Password!))
                throw new AuthenticateException("Invalid Authentication");

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Verifies the user's email using a confirmation token asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user to confirm.</param>
        /// <param name="token">The email confirmation token.</param>
        /// <exception cref="EmailException">Thrown when the user does not exist or email confirmation fails.</exception>
        public async Task EmailCheckAsync(string email, string token)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(email);
            if (user is null)
                throw new EmailException();

            var confirmatedResult = await _unitOfWork.UserAccountRepository().ConfirmEmailAsync(user, token);
            if (!confirmatedResult.Succeeded)
                throw new EmailException();

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Initiates the forgot password process by generating a password reset token and sending it to the user's email asynchronously.
        /// </summary>
        /// <param name="forgotPasswordModel">The forgot password details including email and client URI.</param>
        /// <exception cref="NullReferenceException">Thrown when the user with the provided email does not exist.</exception>
        public async Task ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(forgotPasswordModel.Email!);
            if (user is null)
                throw new NullReferenceException();

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
        }

        /// <summary>
        /// Resets the user's password using a reset token and new password asynchronously.
        /// </summary>
        /// <param name="resetPasswordModel">The reset password details including email, token, and new password.</param>
        /// <exception cref="IdentityException">Thrown when the user does not exist or the password reset fails due to validation errors.</exception>
        public async Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            var errorList = new List<string>();
            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(resetPasswordModel.Email!);
            if (user is null)
            {
                errorList.Add("Invalid Request");
                throw new IdentityException(errorList);
            }

            var result = await _unitOfWork.UserAccountRepository().ResetPasswordAsync(user, resetPasswordModel.Token!, resetPasswordModel.Password!);
            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error => errorList.Add(error.Description));
                throw new IdentityException(errorList);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user account with the specified ID asynchronously.
        /// </summary>
        /// <param name="accountId">The unique identifier of the user account to be deleted.</param>
        /// <exception cref="NullReferenceException">Thrown if the user with the specified ID is not found.</exception>
        /// <exception cref="IdentityException">Thrown if the deletion operation fails.</exception>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAccountAsync(string accountId)
        {
            var user = await _unitOfWork.UserAccountRepository().FindByIdAsync(accountId);

            if (user is null)
                throw new NullReferenceException();

            var result = await _unitOfWork.UserAccountRepository().DeleteAsync(user);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors.Select(x => x.Description).ToList());

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
