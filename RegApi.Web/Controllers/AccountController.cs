using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RegApi.Services.Extensions.Exceptions;
using RegApi.Services.Interfaces;
using RegApi.Services.Models;

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
            try
            {
                await _userService.RegistrateAsync(userRegistrationModel);

                return StatusCode(201);
            }
            catch(IdentityException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationModel userAuthenticationModel)
        {

            await _userService.CheckOfUserAsync(userAuthenticationModel);

            var token = await _jwtService.CreateToken(userAuthenticationModel);

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

            await _userService.ForgotPassword(forgotPasswordModel);

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                throw new Exception();

            await _userService.ResetPassword(resetPasswordModel);

            return Ok();
        }
    }
}
