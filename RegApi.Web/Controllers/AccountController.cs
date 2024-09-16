using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;

namespace RegApi.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IJwtService jwtService, IMapper mapper)
        {
            _userService = userService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel userRegistrationModel)
        {
            await _userService.RegistrateAsync(userRegistrationModel);

            return StatusCode(201);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationModel userAuthenticationModel)
        {

            await _userService.CheckOfUserAsync(userAuthenticationModel);

            var token = await _jwtService.CreateToken(userAuthenticationModel.Email);

            return Ok(new AuthenticationResponsModel { IsAuthSuccessful = true, Token = token });
        }

        [HttpGet("emailconfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            await _userService.EmailCheckAsync(email, token);

            return Ok();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                throw new Exception();

            await _userService.ForgotPasswordAsync(forgotPasswordModel);

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                throw new Exception();

            await _userService.ResetPasswordAsync(resetPasswordModel);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount([FromQuery] string accountId)
        {
            await _userService.DeleteAccountAsync(accountId);

            return Ok();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var props = new AuthenticationProperties { RedirectUri = "/api/accounts/google-auth" };
            
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-auth")]
        public async Task<IActionResult> GoogleLogin()
        {
            var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var email = await _userService.FindOrCreateGoogleAsync(response);

            var token = await _jwtService.CreateToken(email);

            return Ok(new AuthenticationResponsModel { IsAuthSuccessful = true, Token = token });
        }
    }
}
