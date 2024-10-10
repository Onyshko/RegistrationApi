using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using RegApi.Domain.Entities;
using RegApi.Repository.Interfaces;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;
using RegApi.Shared.Extensions.Exceptions;
using RegApi.Shared.Models;
using System.Security.Claims;

namespace RegApi.Services.Implementations
{
    /// <summary>
    /// Provides user-related operations such as registration, authentication, and password management.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for interacting with the repository.</param>
        /// <param name="emailSender">The email sender for sending confirmation and reset emails.</param>
        /// <param name="mapper">The mapper for mapping models to entities.</param>
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
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
            var result = await _unitOfWork.UserAccountRepository().RegisterAsync(user, userRegistrationModel.Password);

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

            var message = new EmailSenderModel()
            {
                Emails = [user.Email!],
                Subject = "Email.Confirmation Token",
                Content = callback
            };

            await _unitOfWork.QueueService().SendMessageAsync(message);

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

            var message = new EmailSenderModel()
            {
                Emails = [user.Email!],
                Subject = "Reset password token",
                Content = callback
            };

            await _unitOfWork.QueueService().SendMessageAsync(message);

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

        /// <summary>
        /// Finds an existing user by their email address or creates a new user based on Google authentication results.
        /// </summary>
        /// <param name="response">The result of Google authentication, including user claims such as email and name.</param>
        /// <returns>The email address of the user that was found or created.</returns>
        /// <exception cref="NullReferenceException">Thrown if the authentication result's principal or email claim is null.</exception>
        /// <exception cref="IdentityException">Thrown if the registration of the new user fails.</exception>
        /// <remarks>
        /// This method first attempts to locate a user in the system using the email address obtained from the Google authentication result.
        /// If the user does not exist, a new user is created with the provided email and name. The new user is then assigned the "Visitor" role.
        /// All changes are committed to the database. The method returns the email address of the user.
        /// </remarks>
        public async Task<string> FindOrCreateGoogleAsync(AuthenticateResult response)
        {
            if (response.Principal is null)
                throw new NullReferenceException();

            var email = response.Principal.FindFirstValue(ClaimTypes.Email);

            var user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(email!);
            if (user is null)
            {
                var newUser = new User
                {
                    FirstName = response.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = response.Principal.FindFirstValue(ClaimTypes.Surname),
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _unitOfWork.UserAccountRepository().RegisterAsync(newUser);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);

                    throw new IdentityException(errors);
                }

                user = await _unitOfWork.UserAccountRepository().FindByEmailAsync(email!);
            }

            await _unitOfWork.UserAccountRepository().AddToRoleAsync(user!, "Visitor");

            await _unitOfWork.SaveChangesAsync();

            return email!;
        }

        /// <summary>
        /// Asynchronously uploads a user's avatar photo to Azure Blob Storage and updates the user's record with the photo's URI.
        /// </summary>
        /// <param name="photo">The avatar photo to upload, represented as an <see cref="IFormFile"/>.</param>
        /// <param name="userId">The ID of the user whose avatar is being uploaded.</param>
        /// <exception cref="NullReferenceException">Thrown if the <paramref name="photo"/> is null.</exception>
        /// <exception cref="Exception">Thrown if the uploaded file is not an image.</exception>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UploadAvatarAsync(IFormFile photo, string userId)
        {
            if (photo == null)
                throw new NullReferenceException();

            if (!photo.ContentType.StartsWith("image/"))
                throw new Exception("File must be image");

            var response = await _unitOfWork.FileService().UploadAsync(photo);

            var user = await _unitOfWork.UserAccountRepository().FindByIdAsync(userId);

            user.AvatarUri = response.Blob.Uri;

            await _unitOfWork.UserAccountRepository().UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
