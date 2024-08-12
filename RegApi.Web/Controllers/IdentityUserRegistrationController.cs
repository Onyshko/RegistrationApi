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
    public class IdentityUserRegistrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public IdentityUserRegistrationController(IUserService userService, IJwtService jwtService, IMapper mapper)
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
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseModel { Errors = errors });
            }

            return StatusCode(201);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationModel userAuthenticationModel)
        {
            if (!await _userService.CheckPassword(userAuthenticationModel))
                return Unauthorized(new AuthenticationResponsModel { ErrorMessage = "Invalid Authentication" });

            var token = await _jwtService.CreateToken(userAuthenticationModel);

            return Ok(new AuthenticationResponsModel { IsAuthSuccessful = true, Token = token });
        }
    }
}
