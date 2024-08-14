using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegApi.Domain.Entities;
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
            if (userRegistrationModel is null)
                return BadRequest();

            var result = await _userService.RegistrateAsync(userRegistrationModel);
            if (result.Count != 0)
            {
                return BadRequest(new RegistrationResponseModel { Errors = result });
            }

            return StatusCode(201);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationModel userAuthenticationModel)
        {
            var responseCheck = await _userService.CheckOfUserAsync(userAuthenticationModel);

            if (responseCheck == "Invalid Request")
            {
                return BadRequest(responseCheck);
            }
            else if (responseCheck != "")
            {
                return Unauthorized(new AuthenticationResponsModel { ErrorMessage = responseCheck });
            }       

            var token = await _jwtService.CreateToken(userAuthenticationModel);

            return Ok(new AuthenticationResponsModel { IsAuthSuccessful = true, Token = token });
        }

        [HttpGet("emailconfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var checkResult = await _userService.EmailCheckAsync(email, token);

            if (checkResult != "")
            {
                return BadRequest(checkResult);
            }

            return Ok();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _userService.ForgotPassword(forgotPasswordModel))
                return BadRequest("Invalid Request");

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.ResetPassword(resetPasswordModel);
            if (result.Count != 0)
                return BadRequest(new { Errors = result });

            return Ok();
        }
    }
}
