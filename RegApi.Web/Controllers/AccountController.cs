using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;

namespace RegApi.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ISasService _sasService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService,
                                 IJwtService jwtService,
                                 ISasService sasService,
                                 IMapper mapper)
        {
            _userService = userService;
            _jwtService = jwtService;
            _sasService = sasService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegistrationModel">The model containing user registration details.</param>
        /// <returns>Returns a status code indicating the result of the operation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel userRegistrationModel)
        {
            await _userService.RegistrateAsync(userRegistrationModel);
            return StatusCode(201);
        }

        /// <summary>
        /// Authenticates a user and returns JWT and SAS tokens.
        /// </summary>
        /// <param name="userAuthenticationModel">The model containing user authentication details.</param>
        /// <returns>Returns a response model with authentication status and tokens.</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationModel userAuthenticationModel)
        {
            await _userService.CheckOfUserAsync(userAuthenticationModel);
            var token = await _jwtService.CreateToken(userAuthenticationModel.Email);
            var sasToken = await _sasService.CreateToken(userAuthenticationModel.Email);
            return Ok(new AuthenticationResponsModel { IsAuthSuccessful = true, Token = token, SasToken = sasToken });
        }

        /// <summary>
        /// Confirms the user's email address using a token.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="token">The confirmation token.</param>
        /// <returns>Returns a status indicating the result of the email confirmation.</returns>
        [HttpGet("emailconfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            await _userService.EmailCheckAsync(email, token);
            return Ok();
        }

        /// <summary>
        /// Initiates the password recovery process by sending a password reset link to the user's email.
        /// </summary>
        /// <param name="forgotPasswordModel">The model containing the user's email.</param>
        /// <returns>Returns a status indicating the result of the operation.</returns>
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                throw new Exception();

            await _userService.ForgotPasswordAsync(forgotPasswordModel);
            return Ok();
        }

        /// <summary>
        /// Resets the user's password using the provided model.
        /// </summary>
        /// <param name="resetPasswordModel">The model containing the user's new password.</param>
        /// <returns>Returns a status indicating the result of the password reset.</returns>
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                throw new Exception();

            await _userService.ResetPasswordAsync(resetPasswordModel);
            return Ok();
        }

        /// <summary>
        /// Deletes a user account.
        /// </summary>
        /// <param name="accountId">The ID of the account to be deleted.</param>
        /// <returns>Returns a status indicating the result of the account deletion.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount([FromQuery] string accountId)
        {
            await _userService.DeleteAccountAsync(accountId);
            return Ok();
        }

        /// <summary>
        /// Initiates Google login process.
        /// </summary>
        /// <returns>Returns a challenge result for Google authentication.</returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            var props = new AuthenticationProperties { RedirectUri = "/api/accounts/google-auth" };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles Google login callback and retrieves the user's email.
        /// </summary>
        /// <returns>Returns a response model with authentication status and JWT token.</returns>
        [HttpGet("google-auth")]
        public async Task<IActionResult> GoogleLogin()
        {
            var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var email = await _userService.FindOrCreateGoogleAsync(response);
            var token = await _jwtService.CreateToken(email);
            return Ok(new AuthenticationResponsModel { IsAuthSuccessful = true, Token = token });
        }

        /// <summary>
        /// Uploads a user's avatar photo.
        /// </summary>
        /// <param name="photo">The photo file to upload.</param>
        /// <returns>Returns a status indicating the result of the photo upload.</returns>
        [Authorize]
        [HttpPost("uploadavatar")]
        public async Task<IActionResult> UploadAvatars(IFormFile photo)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.UploadAvatarAsync(photo, userId);
            return Ok("Photo has been added");
        }
    }
}
